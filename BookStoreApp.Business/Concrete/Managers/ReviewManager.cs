using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.DTOs;
using BookStoreApp.Business.ValidationRules.FluentValidation;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;
using Elastic.Clients.Elasticsearch.MachineLearning;
using FluentValidation;
using FluentValidation.Results;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class ReviewManager : IReviewService
    {
        private readonly IReviewDal _reviewDal;
        private readonly ICacheService _cacheService;
        private readonly IBookService _bookService;
        private readonly IElasticsearchService _elasticsearchService;
        private IValidator<BookReview> _validator;

        public ReviewManager(IReviewDal reviewDal, ICacheService cacheService, IBookService bookService, IElasticsearchService elasticsearchService, IValidator<BookReview> validator)
        {
            _reviewDal = reviewDal;
            _cacheService = cacheService;
            _bookService = bookService;
            _elasticsearchService = elasticsearchService;
            _validator = validator;
        }

        public List<BookReview> AddReview(int id, int bookId, Guid userId, string userName, string reviewText, int rating)
        {
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                throw new ArgumentException("The reviewText field is required.", nameof(reviewText));
            }

            var newReview = _reviewDal.AddReview(id, bookId, userId, userName, reviewText, rating);
            var cacheKey = $"book_{bookId}_reviews";

            // Retrieve existing reviews from cache or create a new list if not present
            var cachedReviews = _cacheService.GetOrAdd(cacheKey, () => new List<BookReview>());

            // Add the new review to the cached list
            cachedReviews.Add(newReview);

            // Update the cache with the new list as a serialized string
            var serializedReviews = JsonSerializer.Serialize(cachedReviews);
            _cacheService.SetValueAsync(cacheKey, serializedReviews);

            // Update the book details in cache and Elasticsearch
            UpdateBookDetails(bookId);

            return cachedReviews;
        }



        public void DeleteReview(int reviewId, Guid userId)
        {
            var review = _reviewDal.Get(r => r.Id == reviewId);
            if (review == null)
            {
                throw new ArgumentException("Review not found.", nameof(reviewId));
            }

            // Check if the user is authorized to delete the review
            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this review.");
            }

            var cacheKey = $"book_{review.BookId}_reviews";
            _cacheService.Clear(cacheKey);
            _reviewDal.Delete(review);

            // Update the book details in cache and Elasticsearch
            UpdateBookDetails(review.BookId);
        }

        public List<BookReview> UpdateReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public List<ReviewModel> GetReviewsByBookId(int bookId)
        {
            var cacheKey = $"book_{bookId}_reviews";
            var cachedReviews = _cacheService.GetOrAdd(cacheKey, () =>
            {
                var reviews = _reviewDal.GetReviewsByBookId(bookId);
                return reviews.Select(r => new ReviewModel
                {
                    Id = r.Id,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    UserName = r.User.UserName,
                    ReviewDate = r.ReviewDate
                }).ToList();
            });

            return cachedReviews;
        }

        private void UpdateBookDetails(int bookId)
        {
            var bookDetails = _bookService.GetBookById(bookId);
            _cacheService.Clear("all_books");
            _cacheService.Clear($"categories_{bookDetails.CategoryName}");
            _cacheService.GetOrAdd("all_books", () => _bookService.GetAllBooks());
            _elasticsearchService.IndexBookAsync(bookDetails).Wait();
        }
    }
}


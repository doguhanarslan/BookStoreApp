using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.DTOs;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class ReviewManager:IReviewService
    {
        private IReviewDal _reviewDal;
        private ICacheService _cacheService;


        public ReviewManager(IReviewDal reviewDal, ICacheService cacheService)
        {
            _reviewDal = reviewDal;
            _cacheService = cacheService;
        }

        public List<BookReview> AddReview(int bookId, int userId, string userName, string reviewText, int rating)
        {
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                throw new ArgumentException("The reviewText field is required.", nameof(reviewText));
            }

            var newReview = _reviewDal.AddReview(bookId, userId, userName, reviewText, rating);
            var cacheKey = $"book_{bookId}_reviews";

            // Retrieve existing reviews from cache or create a new list if not present
            var cachedReviews = _cacheService.GetOrAdd(cacheKey, () => new List<BookReview>());

            // Add the new review to the cached list
            cachedReviews.Add(newReview);

            // Update the cache with the new list as a serialized string
            var serializedReviews = JsonSerializer.Serialize(cachedReviews);
            _cacheService.SetValueAsync(cacheKey, serializedReviews);

            return cachedReviews;
        }

        public List<ReviewModel> GetReviewsByBookId(int bookId)
        {
            var cacheKey = $"book_{bookId}_reviews";
            var cachedReviews = _cacheService.GetOrAdd(cacheKey, () =>
            {
                var reviews = _reviewDal.GetReviewsByBookId(bookId);
                return reviews.Select(r => new ReviewModel
                {
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    UserName = r.User.UserName,
                    ReviewDate = r.ReviewDate
                }).ToList();
            });

            return cachedReviews;
        }
    }
}

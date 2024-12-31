using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public BookReview AddReview(int bookId, int userId, string reviewText, int rating)
        {
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                throw new ArgumentException("Review text is required.", nameof(reviewText));
            }

            // Update the cache after adding the review
            return _cacheService.GetOrAdd($"book_{bookId}_reviews", () => _reviewDal.AddReview(bookId, userId, reviewText, rating));
        }

        public List<ReviewModel> GetReviewsByBookId(int bookId)
        {
            return _cacheService.GetOrAdd($"book_{bookId}_reviews", () =>
            {
                var reviews = _reviewDal.GetReviewsByBookId(bookId);
                return reviews.Select(r => new ReviewModel
                {
                    ReviewText = r.ReviewText,
                    UserName = r.User.UserName,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate
                }).ToList();
            });
        }
    }
}

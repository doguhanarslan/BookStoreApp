using BookStoreApp.Business.DTOs;
using BookStoreApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Business.Abstract
{
    public interface IReviewService
    {
        List<BookReview> AddReview(int id,int bookId,int userId, string userName, string reviewText, int rating);

        public void DeleteReview(int reviewId,int userId);

        List<BookReview> UpdateReview(int reviewId);

        List<ReviewModel> GetReviewsByBookId(int bookId);
    }
}

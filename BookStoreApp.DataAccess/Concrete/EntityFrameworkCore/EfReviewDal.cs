using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfReviewDal : EfEntityRepositoryBase<BookReview, BookstoreContext>, IReviewDal
    {
        private readonly BookstoreContext _context;

        public EfReviewDal(BookstoreContext context)
        {
            _context = context;
        }

        public BookReview AddReview(int id, int bookId, int userId, string userName, string reviewText, int rating)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var existingReview = _context.BookReviews.FirstOrDefault(r => r.BookId == bookId && r.UserId == userId);
            if (existingReview != null)
            {
                throw new InvalidOperationException("User has already reviewed this book");
            }

            var review = new BookReview
            {
                Id = id,
                BookId = bookId,
                UserId = userId,
                UserName = userName,
                ReviewText = reviewText,
                Rating = rating,
                ReviewDate = DateTime.UtcNow
            };

            _context.BookReviews.Add(review);
            _context.SaveChanges();

            return review;
        }


        public List<BookReview> GetReviewsByBookId(int bookId)
        {
            return _context.BookReviews
                .Where(review => review.BookId == bookId)
                .Include(review => review.User) // Eagerly load the User entities
                .ToList();
        }

        public List<BookReview> UpdateReview(int reviewId)
        {
            //var selectedReview = _context.BookReviews.FirstOrDefault(review => review.Id == reviewId);

            return new List<BookReview>();
            //return _context.BookReviews.Update(selectedReview);
        }

        public void DeleteReview(int reviewId)
        {
            var existingReview = _context.BookReviews.FirstOrDefault(r => r.Id == reviewId);

            if (existingReview != null)
            {
                _context.BookReviews.Remove(existingReview);
                _context.SaveChanges();
            }
        }
    } 
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfReviewDal : EfEntityRepositoryBase<BookReview, BookstoreContext>, IReviewDal
    {
        private readonly BookstoreContext _context;

        public EfReviewDal(BookstoreContext context)
        {
            _context = context;
        }

        public List<BookReview> GetAll()
        {
            throw new NotImplementedException();
        }


        public BookReview Update(BookReview entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BookReview entity)
        {
            throw new NotImplementedException();
        }

        public BookReview AddReview(int bookId, int userId, string reviewText, int rating)
        {
            try
            {
                var newReview = new BookReview
                {
                    BookId = bookId,
                    UserId = userId,
                    ReviewText = reviewText,
                    Rating = rating,
                    ReviewDate = DateTime.UtcNow
                };

                _context.BookReviews.Add(newReview);
                _context.SaveChanges();

                return newReview;
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging framework)
                Console.WriteLine($"Error adding review: {ex.Message}");
                throw;
            }
        }

        public List<BookReview> GetReviewsByBookId(int bookId)
        {
            return _context.BookReviews
                .Where(review => review.BookId == bookId)
                .ToList();
        }
    } 
}




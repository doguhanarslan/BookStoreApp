using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Abstract
{
    public interface IReviewDal:IEntityRepository<BookReview>
    {
        List<BookReview> GetReviewsByBookId(int bookId);
        BookReview AddReview(int bookId, int userId, string reviewText, int rating);
    }
}

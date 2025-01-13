using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfBookDal : EfEntityRepositoryBase<Book, BookstoreContext>, IBookDal
    {
        private BookstoreContext _context;
        public EfBookDal(BookstoreContext context)
        {
            _context = context;
        }

        public List<BookDetails> GetAllBooks()
        {
            var result = from b in _context.Books
                         join ba in _context.BookAuthors on b.Id equals ba.BookId
                         join a in _context.Authors on ba.AuthorId equals a.Id
                         join br in _context.BookReviews on b.Id equals br.BookId into brGroup
                         from br in brGroup.DefaultIfEmpty()
                         join u in _context.Users on br.UserName equals u.UserName into uGroup
                         from u in uGroup.DefaultIfEmpty()
                         group new { b, a, br, u } by new { b.Id, a.FirstName, a.LastName, b.Title, b.Description, b.BookImage, b.Price } into g
                         select new
                         {
                             Book = g.Key,
                             AuthorName = g.Key.FirstName + " " + g.Key.LastName,
                             Reviews = g.Select(x => new
                             {
                                 BookId = x.b.Id,
                                 UserName = x.u != null ? x.u.UserName : null,
                                 Rating = x.br != null ? x.br.Rating : (int?)null,
                                 ReviewText = x.br != null ? x.br.ReviewText : null,
                                 ReviewDate = x.br != null ? x.br.ReviewDate : (DateTime?)null,
                                 UserId = x.u != null ? x.u.Id : (int?)null
                             }).ToList(),
                         };

            var bookDetailsList = result.ToList().Select(x => new BookDetails
            {
                BookId = x.Book.Id,
                BookTitle = x.Book.Title,
                BookDescription = x.Book.Description,
                AuthorName = x.AuthorName,
                BookImage = x.Book.BookImage,
                BookPrice = Convert.ToDouble(x.Book.Price),
                BookRate = x.Reviews.Any(r => r.Rating.HasValue) ? (decimal)x.Reviews.Where(r => r.Rating.HasValue).Average(r => r.Rating.Value) : 0,
                BookReviews = x.Reviews.Where(r => r.Rating.HasValue).Select(r => new BookReview
                {
                    BookId = r.BookId,
                    UserName = r.UserName ?? string.Empty,
                    Rating = r.Rating ?? 0,
                    ReviewText = r.ReviewText ?? string.Empty,
                    ReviewDate = r.ReviewDate ?? DateTime.MinValue,
                    UserId = r.UserId ?? 0
                }).ToList()
            }).ToList();

            return bookDetailsList;
        }






        public BookDetails GetBookById(int bookId)
        {
            var book = GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
            //return _context.Books.FirstOrDefault(b => b != null && b.Id == bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }
            return book;

        }


        public BookDetails GetBookByName(string name)
        {
            
            var normalizedBookTitle = NormalizedBookTitle(name);

            var book = GetAllBooks().FirstOrDefault(b => NormalizedBookTitle(b.BookTitle) == normalizedBookTitle);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }
            return book;
        }

        private static string NormalizedBookTitle(string name)
        {
            var normalizedBookTitle = name
                .Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i")
                .Replace("ö", "o").Replace("ş", "s").Replace("ü", "u")
                .ToLower()
                .Replace(" ", "-");
            return normalizedBookTitle;
        }
    }
}

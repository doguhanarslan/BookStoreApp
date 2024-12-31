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
                join u in _context.Users on br.UserId equals u.Id into uGroup
                from u in uGroup.DefaultIfEmpty()
                group new { b, a, br, u } by new { b.Id, a.FirstName, a.LastName, b.Title, b.Description, b.BookImage, b.Price } into g
                select new
                {
                    Book = g.Key,
                    AuthorName = g.Key.FirstName + " " + g.Key.LastName,
                    Reviews = g.Select(x => new
                    {
                        ReviewText = x.br != null ? x.br.ReviewText : null,
                        Rating = x.br != null ? x.br.Rating : (int?)null,
                        UserName = x.u != null ? x.u.UserName : null
                    }).ToList(),
                    ReviewCount = g.Count(x => x.br != null)
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
                ReviewCount = x.ReviewCount,
                ReviewText = string.Join("; ", x.Reviews.Where(r => r.ReviewText != null).Select(r => $"{r.UserName}: {r.ReviewText}"))
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

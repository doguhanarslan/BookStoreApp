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
using BookStoreApp.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

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
            var result = from b in _context.Books.AsNoTracking()
                         join ba in _context.BookAuthors.AsNoTracking() on b.Id equals ba.BookId
                         join a in _context.Authors.AsNoTracking() on ba.AuthorId equals a.Id
                         join br in _context.BookReviews.AsNoTracking() on b.Id equals br.BookId into brGroup
                         from br in brGroup.DefaultIfEmpty()
                         join u in _context.Users.AsNoTracking() on br.UserId equals u.Id into uGroup
                         from u in uGroup.DefaultIfEmpty()
                         group new { b, a, br, u } by new
                         {
                             b.Id,
                             b.Title,
                             b.Description,
                             b.BookImage,
                             b.Price,
                             a.FirstName,
                             a.LastName
                         } into g
                         select new BookDetails
                         {
                             BookId = g.Key.Id,
                             BookTitle = g.Key.Title ?? "No Title",
                             BookDescription = g.Key.Description ?? "No Description",
                             AuthorName = $"{g.Key.FirstName} {g.Key.LastName}",
                             BookImage = g.Key.BookImage ?? "default.png",
                             BookPrice = g.Key.Price,
                             BookRate = g.Any(x => x.br != null) ? g.Where(x => x.br != null).Select(x => x.br.Rating).Average() : 0,
                             BookReviews = g.Where(x => x.br != null).Select(x => new BookReviewDto
                             {
                                 BookId = x.b.Id,
                                 UserName = x.u != null ? x.u.UserName : "Anonymous",
                                 UserId = x.u != null ? x.u.Id : 0,
                                 ReviewText = x.br != null ? x.br.ReviewText : "No Review",
                                 Rating = x.br != null ? x.br.Rating : 0,
                                 ReviewDate = x.br != null ? x.br.ReviewDate : DateTime.MinValue,
                             }).ToList()
                         };

            return result.ToList();
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

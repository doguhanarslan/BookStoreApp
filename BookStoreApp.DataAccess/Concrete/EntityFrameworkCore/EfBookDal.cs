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
            var result = _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookReviews)
                .ThenInclude(br => br.User)
                .Select(b => new BookDetails
                {
                    BookId = b.Id,
                    BookTitle = b.Title ?? "No Title",
                    BookDescription = b.Description ?? "No Description",
                    AuthorName = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)),
                    BookImage = b.BookImage ?? "default.png",
                    BookPrice = b.Price,
                    BookRate = b.BookReviews.Any() ? b.BookReviews.Average(br => br.Rating) : 0,
                    CategoryName = b.Category.CategoryName,
                    BookReviews = b.BookReviews.Select(br => new BookReviewDto
                    {
                        Id = br.Id,
                        BookId = br.BookId,
                        UserName = br.User != null ? br.User.UserName : "Anonymous",
                        UserId = br.User != null ? br.User.Id : 0,
                        ReviewText = br.ReviewText ?? "No Review",
                        Rating = br.Rating,
                        ReviewDate = br.ReviewDate
                    }).ToList()
                })
                .ToList();

            return result;
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

        public List<BookDetails> GetBookByCategory(int categoryId)
        {
            var result = _context.Books
                .AsNoTracking()
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookReviews)
                .ThenInclude(br => br.User)
                .Select(b => new BookDetails
                {
                    BookId = b.Id,
                    BookTitle = b.Title ?? "No Title",
                    BookDescription = b.Description ?? "No Description",
                    AuthorName = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)),
                    BookImage = b.BookImage ?? "default.png",
                    BookPrice = b.Price,
                    BookRate = b.BookReviews.Any() ? b.BookReviews.Average(br => br.Rating) : 0,
                    CategoryName = b.Category.CategoryName,
                    BookReviews = b.BookReviews.Select(br => new BookReviewDto
                    {
                        Id = br.Id,
                        BookId = br.BookId,
                        UserName = br.User != null ? br.User.UserName : "Anonymous",
                        UserId = br.User != null ? br.User.Id : 0,
                        ReviewText = br.ReviewText ?? "No Review",
                        Rating = br.Rating,
                        ReviewDate = br.ReviewDate
                    }).ToList()
                })
                .ToList();

            return result;
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

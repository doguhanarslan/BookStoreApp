using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
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
                         select new BookDetails
                         {
                             AuthorName = (a.FirstName + " " + a.LastName),
                             BookDescription = b.Description,
                             BookId = b.Id,
                             AuthorId = a.Id,
                             BookImage = b.BookImage,
                             BookPrice = Convert.ToDouble(b.Price),
                             BookTitle = b.Title
                         };

            return result.ToList();

        }

        public Book GetBookById(int bookId)
        {
            var book = _context.Set<Book>().SingleOrDefault(b => b.Id == bookId);
            //return _context.Books.FirstOrDefault(b => b != null && b.Id == bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }
            return book;

        }
    }
}

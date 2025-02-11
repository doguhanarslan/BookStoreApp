using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess;
using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfCartDal : EfEntityRepositoryBase<CartItem, BookstoreContext>, ICartDal
    {
        private readonly BookstoreContext _context;

        public EfCartDal(BookstoreContext context)
        {
            _context = context;
        }

        public CartItem? GetCartItem(int bookId, Guid userId)
        {
            return _context.CartItems
                .FirstOrDefault(ci => ci.BookId == bookId && ci.UserId == userId);
        }


        public List<CartItemDetails> GetCartItemsForUserId(Guid userId)
        {
            return (from ci in _context.CartItems
                    join b in _context.Books on ci.BookId equals b.Id
                    join c in _context.Categories on b.CategoryId equals c.CategoryId
                    join ba in _context.BookAuthors on b.Id equals ba.BookId
                    join a in _context.Authors on ba.AuthorId equals a.Id
                    where ci.UserId == userId
                    select new CartItemDetails
                    {
                        Id = ci.Id,
                        BookId = b.Id,
                        CategoryName = c.CategoryName,
                        BookTitle = b.Title,
                        BookDescription = b.Description,
                        BookImage = b.BookImage,
                        Price = b.Price * ci.Quantity,
                        Quantity = ci.Quantity,
                        BookAuthor = $"{a.FirstName} {a.LastName}"
                    }).ToList();
        }
    }
}

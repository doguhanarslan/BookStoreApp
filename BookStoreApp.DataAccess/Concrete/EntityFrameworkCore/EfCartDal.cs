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

        public CartItem? GetCartItem(int bookId, string cartSessionId)
        {
            return _context.CartItems
                .FirstOrDefault(ci => ci.BookId == bookId && ci.CartSessionId == cartSessionId);
        }

        public List<CartItemDetails> GetCartItemsForSession(string cartSessionId)
        {
            return (from ci in _context.CartItems
                    join b in _context.Books on ci.BookId equals b.Id
                    join ba in _context.BookAuthors on b.Id equals ba.BookId
                    join a in _context.Authors on ba.AuthorId equals a.Id
                    where ci.CartSessionId == cartSessionId
                    select new CartItemDetails
                    {
                        Id = ci.Id,
                        BookId = b.Id,
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

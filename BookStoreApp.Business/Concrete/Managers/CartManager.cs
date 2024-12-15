using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class CartManager : ICartService
    {
        private readonly ICartDal _cartDal;
        private readonly IBookDal _bookDal;
        public CartManager(ICartDal cartDal, IBookService bookService, IBookDal bookDal)
        {
            _cartDal = cartDal;
            _bookDal = bookDal;
        }

        public List<Cart> GetAllCarts()
        {
            return null;
        }

        public CartItem AddToCart(int bookId)
        {
            var book = _bookDal.GetAllBooks().FirstOrDefault(b => b.BookId == bookId);
            //var book = _bookDal.GetAllBooks();
            var bookCart = GetBookCartById(bookId);
            CartItem cartItem = new CartItem
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                BookImage = book.BookImage,
                BookDescription = book.BookDescription,
                BookAuthor = book.AuthorName,
                Price = Convert.ToDouble(book.BookPrice),
                Quantity = 1
            };
            return bookCart != null ? UpdateQuantity(bookCart) : _cartDal.Add(cartItem);
        }

        public CartItem UpdateQuantity(CartItem cartItem)
        {
            cartItem.Quantity += 1;
            cartItem.Price += _bookDal.GetBookById(cartItem.BookId).Price;
            return _cartDal.Update(cartItem);
        }


        public CartItem GetBookCartById(int bookId)
        {
            return _cartDal.Get(cart => cart.BookId == bookId);
        }

        public List<CartItem> GetCartItems()
        {
            return _cartDal.GetAll();
        }

        public decimal GetTotalPrice()
        {
            double totalPrice = 0;
            foreach (var cart in _cartDal.GetAll())
            {
                totalPrice += Convert.ToDouble(cart.Price);
            }
            return Convert.ToDecimal(totalPrice);
        }

        public void RemoveFromCart(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}

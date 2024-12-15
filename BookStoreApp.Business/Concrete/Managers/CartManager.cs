using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
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

        public void AddToCart(int bookId, string cartSessionId)
        {
            var existingCartItem = _cartDal.GetCartItem(bookId, cartSessionId);
            var book = _bookDal.GetBookById(bookId);
            if (existingCartItem != null)
            {
                
                existingCartItem.Quantity += 1;
                existingCartItem.Price += book.Price; 
                _cartDal.Update(existingCartItem);
            }
            else
            {
                
                var cartItem = new CartItem
                {
                    CartSessionId = cartSessionId,
                    BookId = book.Id,
                    Quantity = 1,
                    Price = book.Price
                };

                _cartDal.Add(cartItem);
            }
        }

        public CartItem UpdateQuantity(CartItem cartItem)
        {
            throw new NotImplementedException();
        }

        //public CartItemDetails UpdateQuantity(CartItemDetails cartItem)
        //{
        //    cartItem.Quantity += 1;
        //    cartItem.Price += _bookDal.GetBookById(cartItem.BookId).Price;
        //    return _cartDal.Update(cartItem);
        //}


        public CartItem GetBookCartById(int bookId)
        {
            return _cartDal.Get(cart => cart.BookId == bookId);
        }

        public List<CartItemDetails> GetCartItemsForSession(string sessionId)
        {
            return _cartDal.GetCartItemsForSession(sessionId);
        }

        public decimal GetTotalPrice(string cartSessionId)
        {
            double totalPrice = 0;
            foreach (var cart in _cartDal.GetCartItemsForSession(cartSessionId))
            {
                totalPrice += cart.Price;
            }
            return Convert.ToDecimal(totalPrice);
        }

        public void RemoveFromCart(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}

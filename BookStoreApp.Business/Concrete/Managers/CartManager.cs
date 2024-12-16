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
        private readonly IUserDal _userDal;
        public CartManager(ICartDal cartDal, IBookService bookService, IBookDal bookDal, IUserDal userDal)
        {
            _cartDal = cartDal;
            _bookDal = bookDal;
            _userDal = userDal;
        }

        public void AddToCart(int bookId, int userId)
        {
            var existingCartItem = _cartDal.GetCartItem(bookId, userId);
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
                    UserId = userId,
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
            throw new NotImplementedException();
        }

        //public List<CartItemDetails> GetCartItemsForSession(string sessionId)
        //{
        //    return _cartDal.GetCartItemsForSession(sessionId);
        //}

        public List<CartItemDetails> GetCartItemsForUser(int userId)
        {
            var user = _userDal.GetById(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found");
            }
            return _cartDal.GetCartItemsForUserId(user.Id);
        }

        public decimal GetTotalPrice(int userId)
        {
            double totalPrice = 0;
            foreach (var cart in _cartDal.GetCartItemsForUserId(userId))
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

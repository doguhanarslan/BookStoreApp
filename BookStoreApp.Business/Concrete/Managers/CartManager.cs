using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
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
        private readonly ICacheService _cacheService;
        public CartManager(ICartDal cartDal, IBookService bookService, IBookDal bookDal, IUserDal userDal, ICacheService cacheService)
        {
            _cartDal = cartDal;
            _bookDal = bookDal;
            _userDal = userDal;
            _cacheService = cacheService;
        }


        public CartItem? GetCartItem(int bookId, int userId)
        {
            return _cartDal.GetCartItem(bookId, userId);
        }

        public void AddToCart(int bookId, int userId)
        {
            var existingCartItem = _cartDal.GetCartItem(bookId, userId);
            var book = _bookDal.GetBookById(bookId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                existingCartItem.Price += book.Price;

                // Update the existing cart item in the database
                _cartDal.Update(existingCartItem);

                // Update the cache with the modified cart items
                var cartItems = _cartDal.GetCartItemsForUserId(userId);
                _cacheService.SetValueAsync($"cart_items_{userId}", JsonSerializer.Serialize(cartItems));
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

                // Add the new cart item to the database
                _cartDal.Add(cartItem);

                // Update the cache with the new cart items
                var cartItems = _cartDal.GetCartItemsForUserId(userId);
                _cacheService.SetValueAsync($"cart_items_{userId}", JsonSerializer.Serialize(cartItems));
            }
        }



        public List<CartItemDetails> GetCartItemsForUser(int userId)
        {
            return GetCartItemsFromCache(userId);
        }
        public List<CartItemDetails> GetCartItemsFromCache(int userId)
        {
            var user = _userDal.GetById(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found");
            }
            return _cacheService.GetOrAdd($"cart_items_{user.Id}", () => _cartDal.GetCartItemsForUserId(user.Id));
        }


        public CartItem GetBookCartById(int bookId)
        {
            return _cartDal.Get(cart => cart.BookId == bookId);
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

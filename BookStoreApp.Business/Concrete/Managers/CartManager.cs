﻿using System;
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
        private readonly ICacheService _cacheService;
        private readonly IBookService _bookService;
        private readonly IElasticsearchService _elasticsearchService;

        public CartManager(ICartDal cartDal, IBookService bookService, IBookDal bookDal, ICacheService cacheService, IElasticsearchService elasticsearchService)
        {
            _cartDal = cartDal;
            _bookDal = bookDal;
            _cacheService = cacheService;
            _bookService = bookService;
            _elasticsearchService = elasticsearchService;
        }

        public CartItem? GetCartItem(int bookId, Guid userId)
        {
            return _cartDal.GetCartItem(bookId, userId);
        }

        public void AddToCart(int bookId, Guid userId, int quantity)
        {
            var existingCartItem = _cartDal.GetCartItem(bookId, userId);
            var book = _bookDal.GetBookById(bookId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.Price += (book.BookPrice * quantity);

                // Update the existing cart item in the database
                _cartDal.Update(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    BookId = book.BookId,
                    Quantity = quantity,
                    Price = book.BookPrice
                };

                // Add the new cart item to the database
                _cartDal.Add(cartItem);
            }

            // Update the cache with the modified cart items
            UpdateCartCache(userId);
        }

        public List<CartItemDetails> GetCartItemsForUser(Guid userId)
        {
            return GetCartItemsFromCache(userId);
        }

        public List<CartItemDetails> GetCartItemsFromCache(Guid userId)
        {
            
            return _cacheService.GetOrAdd($"cart_items_{userId}", () => _cartDal.GetCartItemsForUserId(userId));
        }

        public CartItem GetBookCartById(int bookId)
        {
            return _cartDal.Get(cart => cart.BookId == bookId);
        }

        public decimal GetTotalPrice(Guid userId)
        {
            double totalPrice = 0;
            foreach (var cart in _cartDal.GetCartItemsForUserId(userId))
            {
                totalPrice += cart.Price;
            }
            return Convert.ToDecimal(totalPrice);
        }

        public void RemoveFromCart(int bookId, Guid userId)
        {
            var cartItem = _cartDal.GetCartItem(bookId, userId);
            if (cartItem != null)
            {
                // Remove the cart item from the database
                _cartDal.Delete(cartItem);

                // Update the cache with the modified cart items
                UpdateCartCache(userId);
            }
        }

        private void UpdateCartCache(Guid userId)
        {
            _cacheService.Clear($"cart_items_{userId}");
            var cartItems = _cartDal.GetCartItemsForUserId(userId);
            _cacheService.GetOrAdd($"cart_items_{userId}", () => cartItems);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Abstract
{
    public interface ICartService
    {
        public void AddToCart(int bookId, int userId);
        CartItem UpdateQuantity(CartItem cartItem);
        CartItem GetBookCartById(int cartId);
        List<CartItemDetails> GetCartItemsForSession(string sessionId);

        List<CartItemDetails> GetCartItemsForUser(int userId);
        decimal GetTotalPrice(int userId);
        void RemoveFromCart(int bookId);
    }
}

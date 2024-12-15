using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Abstract
{
    public interface ICartService
    {
        List<Cart> GetAllCarts();

        CartItem AddToCart(int bookId);
        CartItem UpdateQuantity(CartItem cartItem);
        CartItem GetBookCartById(int cartId);
        List<CartItem> GetCartItems();
        decimal GetTotalPrice();
        void RemoveFromCart(int bookId);
    }
}

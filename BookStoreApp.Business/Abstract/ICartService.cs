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
        public void AddToCart(int bookId, Guid userId,int quantity);
        CartItem GetBookCartById(int cartId);
        List<CartItemDetails> GetCartItemsForUser(Guid userId);
        decimal GetTotalPrice(Guid userId);
        void RemoveFromCart(int bookId,Guid userId);
    }
}

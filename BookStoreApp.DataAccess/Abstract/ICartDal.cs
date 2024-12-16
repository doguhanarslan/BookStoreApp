using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Abstract
{
    public interface ICartDal:IEntityRepository<CartItem>
    {
        public CartItem? GetCartItem(int bookId, int userId);
        //public List<CartItemDetails> GetCartItemsForSession(string cartSessionId);
        public List<CartItemDetails> GetCartItemsForUserId(int userId);
    }
}

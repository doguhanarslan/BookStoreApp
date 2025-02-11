using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class CartItem:IEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        //public string CartSessionId { get; set; }

        public Guid UserId { get; set; }

        public double Price { get; set; }
        public virtual Book Book { get; set; }
    }
}

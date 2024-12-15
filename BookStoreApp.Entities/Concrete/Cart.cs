using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class Cart:IEntity
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public int BookQuantity { get; set; }
    }
}

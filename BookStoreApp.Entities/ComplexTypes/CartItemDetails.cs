using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Entities.ComplexTypes
{
    public class CartItemDetails
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }

        public string CategoryName { get; set; }
        public string BookDescription { get; set; }
        public string BookImage { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string BookAuthor { get; set; }
    }
}

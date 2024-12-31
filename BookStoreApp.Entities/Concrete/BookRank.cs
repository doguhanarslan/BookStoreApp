using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class BookRank:IEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public decimal Rate
        {
            get
            {
                if (Book?.BookReviews == null || !Book.BookReviews.Any())
                {
                    return 0;
                }
                return (decimal)Book.BookReviews.Average(br => br.Rating);
            }
        }
    }
}

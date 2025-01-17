using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;
using BookStoreApp.Entities.DTOs;

namespace BookStoreApp.Entities.ComplexTypes
{
    public class BookDetails
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }

        public string CategoryName { get; set; }
        public string BookImage { get; set; }
        public string BookDescription { get; set; }
        public string AuthorName { get; set; }
        public double BookPrice { get; set; }
        public double BookRate { get; set; }

        public List<BookReviewDto> BookReviews { get; set; }

    }
}

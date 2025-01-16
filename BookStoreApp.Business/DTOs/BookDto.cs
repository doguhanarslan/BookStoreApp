using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Business.DTOs
{
    public class SearchDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }

        public string BookDescription { get; set; }
        public string AuthorName { get; set; }
    }
}

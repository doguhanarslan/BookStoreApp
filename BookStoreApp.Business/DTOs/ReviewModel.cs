using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Business.DTOs
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}

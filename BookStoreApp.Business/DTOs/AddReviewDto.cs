using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Business.DTOs
{
    public class AddReviewDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
}

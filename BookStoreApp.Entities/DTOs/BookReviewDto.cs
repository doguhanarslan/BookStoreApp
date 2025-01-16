using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.DTOs
{
    public class BookReviewDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}

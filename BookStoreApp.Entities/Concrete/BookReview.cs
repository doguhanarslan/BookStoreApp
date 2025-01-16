using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class BookReview : IEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserName { get; set; }

        public int UserId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }
    }
}

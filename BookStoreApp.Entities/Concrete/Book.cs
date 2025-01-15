using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class Book : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public string BookImage { get; set; }
        public int Page { get; set; }
        public double Price { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookReview> BookReviews { get; set; }

    }
}

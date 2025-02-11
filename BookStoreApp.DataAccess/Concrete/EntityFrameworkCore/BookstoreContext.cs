using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.DataAccess.Concrete.EntityFrameworkCore.Mappings;
using BookStoreApp.Entities.Concrete;
using BookStoreApp.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class BookstoreContext:DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options):base(options)
        {
            
        }

        public BookstoreContext()
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookReview> BookReviews { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BookStoreDb;Username=postgres;Password=postgres");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookMap());
            modelBuilder.ApplyConfiguration(new AuthorMap());
            modelBuilder.ApplyConfiguration(new BookAuthorMap());
            modelBuilder.ApplyConfiguration(new CartMap());
            modelBuilder.ApplyConfiguration(new CartItemMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new BookReviewMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
        }
    }
}

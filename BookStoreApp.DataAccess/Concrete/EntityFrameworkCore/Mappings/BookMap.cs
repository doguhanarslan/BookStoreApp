using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore.Mappings
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(@"Book", @"public");
            builder.HasKey(b => b.Id);


            builder.Property(b => b.Id).HasColumnName("BookId");
            builder.Property(b => b.PublisherId).HasColumnName("PublisherId");
            builder.Property(b => b.Title).HasColumnName("Title");
            builder.Property(b => b.Price).HasColumnName("Price");
            builder.Property(b => b.Page).HasColumnName("Page");
            builder.Property(b => b.Description).HasColumnName("Description");
            builder.Property(b => b.BookImage).HasColumnName("BookImage");
            builder.Property(b => b.Isbn).HasColumnName("Isbn");



            //builder.HasMany(b => b.Authors).WithMany(a => a.Books).UsingEntity<Dictionary<string, object>>("BookAuthor",
            //    b => b.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
            //    a => a.HasOne<Book>().WithMany().HasForeignKey("BookId"));
        }
    }
}

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
    public class CartItemMap:IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable(@"CartItems", @"public");
            builder.HasKey(b => b.Id);


            builder.Property(b => b.BookId).HasColumnName("BookId");
            builder.Property(b => b.BookTitle).HasColumnName("BookTitle");
            builder.Property(b => b.BookDescription).HasColumnName("BookDescription");
            builder.Property(b => b.Price).HasColumnName("Price");
            builder.Property(b => b.Quantity).HasColumnName("Quantity");
            builder.Property(b => b.BookAuthor).HasColumnName("BookAuthor");
            builder.Property(b => b.BookImage).HasColumnName("BookImage");


        }
    }
}

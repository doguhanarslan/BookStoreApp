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
    public class BookReviewMap: IEntityTypeConfiguration<BookReview>
    {
        public void Configure(EntityTypeBuilder<BookReview> builder)
        {
            builder.ToTable(@"BookReviews", @"public");
            builder.HasKey(br => br.Id);

            builder.Property(br => br.Id).HasColumnName("Id");
            builder.Property(br => br.BookId).HasColumnName("BookId");
            builder.Property(br => br.UserId).HasColumnName("UserId");
            builder.Property(br => br.ReviewText).HasColumnName("ReviewText");
            builder.Property(br => br.Rating).HasColumnName("Rating");
            builder.Property(br => br.ReviewDate).HasColumnName("ReviewDate");
        }
    }
}

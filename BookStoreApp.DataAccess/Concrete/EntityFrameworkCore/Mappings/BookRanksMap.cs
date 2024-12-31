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
    public class BookRanksMap:IEntityTypeConfiguration<BookRank>
    {
        public void Configure(EntityTypeBuilder<BookRank> builder)
        {
            builder.ToTable(@"BookRanks", @"public");
            builder.HasKey(br => br.Id);

            builder.Property(br => br.Id).HasColumnName("Id");
            builder.Property(br => br.BookId).HasColumnName("BookId");
        }
    }
}

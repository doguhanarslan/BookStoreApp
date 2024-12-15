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
    internal class AuthorMap:IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable(@"Author", @"public");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).HasColumnName("Id");
            builder.Property(a => a.FirstName).HasColumnName("FirstName");
            builder.Property(a => a.LastName).HasColumnName("LastName");
        }
    }
}

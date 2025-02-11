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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(@"User", @"public");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("Id");
            builder.Property(u => u.FirstName).HasColumnName("FirstName");
            builder.Property(u => u.LastName).HasColumnName("LastName");
            builder.Property(u => u.Email).HasColumnName("Email");
            builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash");
            builder.Property(u => u.ProfileImage).HasColumnName("ProfileImage");
        }
    }
}

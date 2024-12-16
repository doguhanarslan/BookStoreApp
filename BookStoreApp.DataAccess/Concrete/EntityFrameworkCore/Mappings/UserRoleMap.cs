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
    public class UserRoleMap:IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(@"UserRole", @"public");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("Id");
            builder.Property(u => u.UserId).HasColumnName("UserId");
            builder.Property(u => u.RoleId).HasColumnName("RoleId");
        }
    }
}

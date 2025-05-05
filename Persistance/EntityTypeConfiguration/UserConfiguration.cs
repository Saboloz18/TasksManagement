using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Users;

namespace TasksManagement.Persistance.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x=>x.CreateDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Name).IsUnique();

        }
    }
}

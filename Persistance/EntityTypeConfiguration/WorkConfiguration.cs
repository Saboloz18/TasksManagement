using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;

namespace TasksManagement.Persistance.EntityTypeConfiguration
{
    public class WorkConfiguration : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.CreateDate).HasDefaultValue(DateTime.Now);    
            builder.HasIndex(w => w.Title).IsUnique();

            builder.HasOne(w => w.CurrentUser)
                .WithMany(u => u.AssignedWork)
                .HasForeignKey(w => w.CurrentUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(w => w.AssignmentHistory)
                .WithOne()
                .HasForeignKey(wa => wa.WorkId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

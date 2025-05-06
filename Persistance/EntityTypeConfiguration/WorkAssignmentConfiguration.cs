using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Persistance.EntityTypeConfiguration
{
    public class WorkAssignmentConfiguration : IEntityTypeConfiguration<WorkAssignment>
    {
        public void Configure(EntityTypeBuilder<WorkAssignment> builder)
        {
            builder.Property(x=>x.CreateDate).HasDefaultValue(DateTime.Now);
            builder.HasOne(wa => wa.User)
                .WithMany()
                .HasForeignKey(wa => wa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wa => wa.Work)
                .WithMany(w => w.AssignmentHistory)
                .HasForeignKey(wa => wa.WorkId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

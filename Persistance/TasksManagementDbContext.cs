using Microsoft.EntityFrameworkCore;
using TasksManagement.Domain.Users;
using TasksManagement.Domain.WorkAssignments;
using TasksManagement.Domain.Works;

namespace TasksManagement.Persistance
{
    public class TaskManagementDbContext : DbContext
    {
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<WorkAssignment> WorkAssignments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
                
        }
    }
}

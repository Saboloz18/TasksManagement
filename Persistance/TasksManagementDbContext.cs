using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TasksManagement.Domain.SystemUsers;
using TasksManagement.Domain.Users;
using TasksManagement.Domain.WorkAssignments;
using TasksManagement.Domain.Works;


public class TaskManagementDbContext : IdentityDbContext<SystemUser>
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
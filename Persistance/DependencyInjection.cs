using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksManagement.Domain.SystemUsers;
using TasksManagement.Persistance.Repositories.Users;
using TasksManagement.Persistance.Repositories.WorkAssignments;
using TasksManagement.Persistance.Repositories.Works;


namespace TasksManagement.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContext<TaskManagementDbContext>(options => options.UseSqlServer(dbConnectionString));

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
                dbContext.Database.Migrate();
            }
            services.AddIdentity<SystemUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<TaskManagementDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IWorkRepository, WorkRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkAssignmentRepository, WorkAssignmentRepository>();
            services.AddScoped<Seed>();

            return services;
        }
    }

}
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TasksManagement.Domain.SystemUsers;

namespace TasksManagement.Infrastructure
{
    public static class DependncyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

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

            return services;

        }
    }
}

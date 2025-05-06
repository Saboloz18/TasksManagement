using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TasksManagement.Domain.SystemUsers;

namespace TasksManagement.Persistence
{
    public class Seed
    {
        private readonly IServiceProvider _serviceProvider;

        public Seed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAdminUserAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SystemUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            var adminSystemUser = await userManager.FindByNameAsync("admin");
            if (adminSystemUser == null)
            {
                adminSystemUser = new SystemUser
                {
                    UserName = "admin",
                };
                var result = await userManager.CreateAsync(adminSystemUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminSystemUser, "Admin");
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

}
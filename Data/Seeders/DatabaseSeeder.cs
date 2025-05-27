using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Seeders;
public class DatabaseSeeder
{
  public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = ["Admin", "User"];

    foreach (var roleName in roleNames)
    {
      if (!await roleManager.RoleExistsAsync(roleName))
      {
        await roleManager.CreateAsync(new IdentityRole(roleName));
      }
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    string adminEmail = "admin@domain.com";
    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

    if (existingAdmin == null)
    {
      var adminUser = new UserEntity
      {
        UserName = adminEmail,
        Email = adminEmail,
        FirstName = "System",
        LastName = "Admin",
        EmailConfirmed = true,
      };
      var result = await userManager.CreateAsync(adminUser, "Admin!123");

      if (result.Succeeded)
        await userManager.AddToRolesAsync(adminUser, roleNames);
    }
  }
}

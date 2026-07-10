using Microsoft.AspNetCore.Identity;
using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                  
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            
            string adminEmail = "admin@store.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                   
                    FirstName = "System",
                    LastName = "Admin",
                    City = "Default",
                    ShippingAddress = "Default",
                    PostalCode = "00000"
                };

                var createPowerUser = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (createPowerUser.Succeeded)
                {
                  
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
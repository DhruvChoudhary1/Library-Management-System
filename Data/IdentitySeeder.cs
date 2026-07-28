using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // =====================================================
            // CREATE ROLES
            // =====================================================

            string[] roles =
            {
                "Admin",
                "Librarian"
            };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(roleName));
                }
            }

            // =====================================================
            // CREATE DEFAULT ADMIN
            // =====================================================

            const string adminEmail = "admin@library.com";
            const string adminPassword = "Admin123";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FullName = "Library Administrator",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        adminUser,
                        adminPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description));

                    throw new Exception(
                        $"Unable to create default admin: {errors}");
                }
            }

            // =====================================================
            // ASSIGN ADMIN ROLE
            // =====================================================

            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    "Admin"))
            {
                var result =
                    await userManager.AddToRoleAsync(
                        adminUser,
                        "Admin");

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description));

                    throw new Exception(
                        $"Unable to assign Admin role: {errors}");
                }
            }
        }
    }
}
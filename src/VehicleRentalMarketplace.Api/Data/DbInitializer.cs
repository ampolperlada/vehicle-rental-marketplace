using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Helpers;

namespace VehicleRentalMarketplace.Api.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "User" },  
                    new Role { RoleName = "Admin" }
                );
                context.SaveChanges();
            }

            var userRole = context.Roles.FirstOrDefault(r => r.RoleName == "User" || r.RoleName == "Customer");
            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");

            if (!context.Users.Any() && userRole != null && adminRole != null)
            {
                context.Users.AddRange(
                    new User
                    {
                        Username = "ampol",
                        Password = PasswordHelper.HashPassword("Password123!"),                        Email = "ampol@vehiclerental.com",
                        Firstname = "Ampol",
                        Lastname = "Admin",
                        RoleID = adminRole.RoleID 
                    },
                    new User
                    {
                        Username = "luwigie",
                        Password = PasswordHelper.HashPassword("Password123!"),                        Email = "luwigie@vehiclerental.com",
                        Firstname = "Luwigie",
                        Lastname = "User",
                        RoleID = userRole.RoleID 
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
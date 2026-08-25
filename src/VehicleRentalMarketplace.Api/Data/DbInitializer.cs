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

            // Seed Roles (Admin, Renter, Buyer)
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Renter" },
                    new Role { RoleName = "Buyer" }
                );
                context.SaveChanges();
            }

            // Get roles
            var ownerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            var renterRole = context.Roles.FirstOrDefault(r => r.RoleName == "Renter");
            var buyerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Buyer");

            // Seed Users
            if (!context.Users.Any())
            {
                var users = new List<User>();

                // Admin User
                if (ownerRole != null)
                {
                    users.Add(new User
                    {
                        Username = "Owners",
                        Password = PasswordHelper.HashPassword("Password123!"),
                        Email = "owners@vehiclerental.com",
                        Firstname = "Ampol",
                        Lastname = "Owners",
                        RoleID = ownerRole.RoleID,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                // Renter User
                if (renterRole != null)
                {
                    users.Add(new User
                    {
                        Username = "renter1",
                        Password = PasswordHelper.HashPassword("Password123!"),
                        Email = "renter@vehiclerental.com",
                        Firstname = "Renter",
                        Lastname = "User",
                        RoleID = renterRole.RoleID,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                // Buyer User
                if (buyerRole != null)
                {
                    users.Add(new User
                    {
                        Username = "buyer1",
                        Password = PasswordHelper.HashPassword("Password123!"),
                        Email = "buyer@vehiclerental.com",
                        Firstname = "Buyer",
                        Lastname = "User",
                        RoleID = buyerRole.RoleID,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
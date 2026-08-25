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

            // Seed Categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Vehicle", Description = "Cars, motorcycles, vans, trucks" },
                    new Category { Name = "Motorcycle", Description = "Scooters, bikes, motorcycles" },
                    new Category { Name = "Van", Description = "Passenger and cargo vans" },
                    new Category { Name = "Truck", Description = "Pickup trucks, cargo trucks" },
                    new Category { Name = "SUV", Description = "Sports Utility Vehicles" },
                    new Category { Name = "Luxury", Description = "Luxury and premium vehicles" }
                );
                context.SaveChanges();
            }

            // Seed ListingTypes
            if (!context.ListingTypes.Any())
            {
                context.ListingTypes.AddRange(
                    new ListingType { Name = "Rent", Description = "Available for rent only" },
                    new ListingType { Name = "Sale", Description = "Available for sale only" },
                    new ListingType { Name = "Both", Description = "Available for both rent and sale" }
                );
                context.SaveChanges();
            }

            // Seed Roles
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
            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            var renterRole = context.Roles.FirstOrDefault(r => r.RoleName == "Renter");
            var buyerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Buyer");

            // Get categories
            var vehicleCategory = context.Categories.FirstOrDefault(c => c.Name == "Vehicle");
            var motorcycleCategory = context.Categories.FirstOrDefault(c => c.Name == "Motorcycle");

            // Get listing types
            var rentType = context.ListingTypes.FirstOrDefault(l => l.Name == "Rent");
            var saleType = context.ListingTypes.FirstOrDefault(l => l.Name == "Sale");
            var bothType = context.ListingTypes.FirstOrDefault(l => l.Name == "Both");

            // Seed Users
            if (!context.Users.Any())
            {
                var users = new List<User>();

                // Admin User
                if (adminRole != null)
                {
                    users.Add(new User
                    {
                        Username = "ampol",
                        Password = PasswordHelper.HashPassword("Password123!"),
                        Email = "ampol@vehiclerental.com",
                        Firstname = "Ampol",
                        Lastname = "Admin",
                        RoleID = adminRole.RoleID,
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

            // Seed Sample Assets (if none)
            if (!context.Assets.Any())
            {
                var adminUser = context.Users.FirstOrDefault(u => u.Username == "ampol");

                if (adminUser != null && vehicleCategory != null && rentType != null)
                {
                    context.Assets.AddRange(
                        new Asset
                        {
                            UserID = adminUser.UserID,
                            Title = "Toyota Vios 2020",
                            Description = "Good condition, well-maintained",
                            CategoryId = vehicleCategory.CategoryId,
                            ListingTypeId = rentType.ListingTypeId,
                            DailyRate = 1500,
                            SalePrice = null,
                            Location = "Manila",
                            IsAvailable = true,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Asset
                        {
                            UserID = adminUser.UserID,
                            Title = "Honda Civic 2021",
                            Description = "Low mileage, 1st owner",
                            CategoryId = vehicleCategory.CategoryId,
                            ListingTypeId = saleType.ListingTypeId,
                            DailyRate = null,
                            SalePrice = 680000,
                            Location = "Makati",
                            IsAvailable = true,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
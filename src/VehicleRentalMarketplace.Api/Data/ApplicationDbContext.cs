using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Helpers;

namespace VehicleRentalMarketplace.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Seed Roles (ID 1, 2, 3)
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin" },
                new Role { RoleID = 2, RoleName = "Vendor" },
                new Role { RoleID = 3, RoleName = "Customer" }
            );

            // Seed Admin User
            //modelBuilder.Entity<User>().HasData(
            //     new User
            //     {
            //         UserID = 1,
            //         Username = "admin",
            //         Email = "admin@vehiclemarketplace.com",
            //         Password = PasswordHelper.HashPassword("Admin@123"),
            //         Firstname = "System",
            //         Lastname = "Admin",
            //         PhoneNumber = "09123456789",
            //         Address = "Admin Address",
            //         City = "Manila",
            //         State = "Metro Manila",
            //         RoleID = 1,
            //         isActive = true,
            //         CreatedAt = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc), 
            //         UpdatedAt = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc) 
            //     }
            // );
        }
    }
}
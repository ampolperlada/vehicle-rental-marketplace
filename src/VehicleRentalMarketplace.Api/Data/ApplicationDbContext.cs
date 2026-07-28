using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Models;

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

            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var vendorRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var customerRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = adminRoleId, RoleName = "Admin" },
                new Role { RoleID = vendorRoleId, RoleName = "Vendor" },
                new Role { RoleID = customerRoleId, RoleName = "Customer" }
            );
        }
    }
}
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Models;

namespace VehicleRentalMarketplace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ListingType> ListingTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RELATIONSHIP CONFIGURATIONS

            // User - Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);

            // Asset - User relationship
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assets)
                .HasForeignKey(a => a.RenterID);

            // Asset - Category relationship
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Assets)
                .HasForeignKey(a => a.CategoryID);

            // Asset - ListingType relationship
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.ListingType)
                .WithMany(lt => lt.Assets)
                .HasForeignKey(a => a.ListingTypeID);

            // Category - ListingType relationship
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ListingType)
                .WithMany(lt => lt.Categories)
                .HasForeignKey(c => c.ListingTypeID);

            // Purchase - Asset relationship
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Asset)
                .WithMany(a => a.Purchases)
                .HasForeignKey(p => p.BookingID);

            // Purchase - User relationship
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserID);

            // Payment - Asset relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Asset)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.BookingID);

            // Payment - Purchase relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Purchase)
                .WithMany(pr => pr.Payments)
                .HasForeignKey(p => p.PurchasedID);

            // Review - Payment relationship
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Payment)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PaymentID);

            // Review - User relationship
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID);

            // SEED DATA

            // 1. SEED ROLES
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin", Description = "Full system access", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Role { RoleID = 2, RoleName = "User", Description = "Regular user - can view assets", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Role { RoleID = 3, RoleName = "Seller", Description = "Can list and manage assets", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Role { RoleID = 4, RoleName = "Moderator", Description = "Can approve and moderate content", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // 2. SEED LISTING TYPES
            modelBuilder.Entity<ListingType>().HasData(
                new ListingType
                {
                    ListingTypeID = 1,
                    TypeName = "Rent",
                    Description = "Vehicles available for rent only",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new ListingType
                {
                    ListingTypeID = 2,
                    TypeName = "Sale",
                    Description = "Vehicles available for purchase only",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new ListingType
                {
                    ListingTypeID = 3,
                    TypeName = "Both",
                    Description = "Vehicles available for both rent and purchase",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // 3. SEED CATEGORIES
            modelBuilder.Entity<Category>().HasData(
                // Rent categories
                new Category { CategoryID = 1, CategoryName = "Sedan", ListingTypeID = 1, Description = "Standard 4-door passenger cars", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 2, CategoryName = "SUV", ListingTypeID = 1, Description = "Sport Utility Vehicles - spacious and versatile", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 3, CategoryName = "Luxury", ListingTypeID = 1, Description = "Premium luxury vehicles for rent", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 4, CategoryName = "Motorcycle", ListingTypeID = 1, Description = "Two-wheeled motor vehicles for rent", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 5, CategoryName = "Van", ListingTypeID = 1, Description = "Vans for passenger or cargo transport", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 6, CategoryName = "Electric", ListingTypeID = 1, Description = "Electric vehicles (EVs) for rent", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },

                // Sale categories
                new Category { CategoryID = 7, CategoryName = "Truck", ListingTypeID = 2, Description = "Pickup trucks for hauling and heavy duty", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 8, CategoryName = "Classic", ListingTypeID = 2, Description = "Classic and vintage cars for sale", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 9, CategoryName = "Sports Car", ListingTypeID = 2, Description = "High-performance sports cars", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },

                // Both categories
                new Category { CategoryID = 10, CategoryName = "Luxury", ListingTypeID = 3, Description = "Premium luxury vehicles for rent or purchase", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 11, CategoryName = "Electric", ListingTypeID = 3, Description = "Electric vehicles (EVs) for rent or purchase", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 12, CategoryName = "Convertible", ListingTypeID = 3, Description = "Convertible/Open top vehicles", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new Category { CategoryID = 13, CategoryName = "SUV", ListingTypeID = 3, Description = "SUVs available for both rent and purchase", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // 4. SEED USERS
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    RoleID = 1,
                    Username = "admin",
                    PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Admin123!")),
                    Firstname = "System",
                    Lastname = "Administrator",
                    Email = "admin@vehiclemarketplace.com",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserID = 2,
                    RoleID = 2,
                    Username = "john_user",
                    PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("User123!")),
                    Firstname = "John",
                    Lastname = "User",
                    Email = "john@example.com",
                    PhoneNumber = "123-456-7890",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserID = 3,
                    RoleID = 3,
                    Username = "jane_seller",
                    PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Seller123!")),
                    Firstname = "Jane",
                    Lastname = "Seller",
                    Email = "jane@seller.com",
                    PhoneNumber = "987-654-3210",
                    Address = "123 Main St",
                    City = "Los Angeles",
                    State = "CA",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserID = 4,
                    RoleID = 4,
                    Username = "moderator",
                    PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Mod123!")),
                    Firstname = "Mod",
                    Lastname = "Moderator",
                    Email = "mod@moderator.com",
                    PhoneNumber = "555-555-5555",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            // 5. SEED SAMPLE ASSETS

            // Assets listed by "jane_seller" (UserID = 3)
            // Some approved, some pending

            modelBuilder.Entity<Asset>().HasData(
                // Approved Rent Asset
                new Asset
                {
                    AssetID = 1,
                    RenterID = 3, // jane_seller
                    Title = "2024 Toyota Camry",
                    Description = "Well-maintained sedan with leather seats, navigation, and backup camera. Perfect for business trips or family outings.",
                    CategoryID = 1, // Sedan
                    ListingTypeID = 1, // Rent
                    DailyRate = 45.00m,
                    SalePrice = null,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Approved",
                    ApproveBy = 1, // admin
                    Status = "Available",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                },
                // Approved Sale Asset
                new Asset
                {
                    AssetID = 2,
                    RenterID = 3, // jane_seller
                    Title = "2019 Ford F-150",
                    Description = "Powerful pickup truck with towing package, 4x4 capability, and bed liner. Great for construction or outdoor work.",
                    CategoryID = 7, // Truck
                    ListingTypeID = 2, // Sale
                    DailyRate = null,
                    SalePrice = 35000.00m,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Approved",
                    ApproveBy = 1, // admin
                    Status = "Available",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                },
                // Approved Both (Rent & Sale)
                new Asset
                {
                    AssetID = 3,
                    RenterID = 3, // jane_seller
                    Title = "2023 Tesla Model 3",
                    Description = "Electric vehicle with autopilot, panoramic roof, and premium sound system. Zero emissions, instant torque.",
                    CategoryID = 10, // Luxury (Both)
                    ListingTypeID = 3, // Both
                    DailyRate = 85.00m,
                    SalePrice = 48000.00m,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Approved",
                    ApproveBy = 1, // admin
                    Status = "Rented",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                },
                // Pending Asset (Needs Approval)
                new Asset
                {
                    AssetID = 4,
                    RenterID = 3, // jane_seller
                    Title = "2022 Jeep Wrangler",
                    Description = "Off-road capable SUV with removable doors and roof. Perfect for adventure seekers.",
                    CategoryID = 2, // SUV (Rent)
                    ListingTypeID = 1, // Rent
                    DailyRate = 75.00m,
                    SalePrice = null,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Pending",
                    ApproveBy = null,
                    Status = "Available",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                },
                // Another Approved Asset
                new Asset
                {
                    AssetID = 5,
                    RenterID = 3, // jane_seller
                    Title = "2021 Chevrolet Corvette",
                    Description = "Mid-engine sports car with 490 horsepower, premium audio, and performance exhaust.",
                    CategoryID = 9, // Sports Car (Sale)
                    ListingTypeID = 2, // Sale
                    DailyRate = null,
                    SalePrice = 68000.00m,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Approved",
                    ApproveBy = 1, // admin
                    Status = "Available",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                },
                // Another Rent Asset (Different User - would need another seller)
                // Since we only have one seller, we'll add one more with the same seller
                new Asset
                {
                    AssetID = 6,
                    RenterID = 3, // jane_seller
                    Title = "2024 Honda CR-V",
                    Description = "Compact SUV with hybrid engine, spacious cargo area, and Honda Sensing safety features.",
                    CategoryID = 13, // SUV (Both)
                    ListingTypeID = 3, // Both
                    DailyRate = 55.00m,
                    SalePrice = 32000.00m,
                    Location = "Los Angeles, CA",
                    ApprovalStatus = "Approved",
                    ApproveBy = 1, // admin
                    Status = "Sold",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = null
                }
            );

            // 6. SEED PURCHASES (Optional - for testing)

            modelBuilder.Entity<Purchase>().HasData(
                new Purchase
                {
                    PurchasedID = 1,
                    BookingID = 3, // Tesla Model 3
                    PurchasePrice = 48000.00m,
                    Date = new DateTime(2024, 1, 1).AddDays(-5),
                    UserID = 2 // john_user bought it
                }
            );

            // 7. SEED PAYMENTS (Optional - for testing)

            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentID = 1,
                    BookingID = 3, // Tesla Model 3
                    PurchasedID = 1,
                    PaymentMethod = "Credit Card",
                    Status = "Completed",
                    TransactionNumber = "TXN-2026-001",
                    PaidAt = new DateTime(2024, 1, 1).AddDays(-5)
                }
            );

            // 8. SEED REVIEWS (Optional - for testing)

            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewerID = 1,
                    PaymentID = 1,
                    Rating = 5,
                    Comment = "Excellent car! Very clean and well-maintained. Will rent again.",
                    UserID = 2, // john_user
                    CreatedAt = new DateTime(2024, 1, 1).AddDays(-4)
                }
            );
        }
    }
}
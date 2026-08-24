using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleRentalMarketplace.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingTypes",
                columns: table => new
                {
                    ListingTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTypes", x => x.ListingTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ListingTypeID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_ListingTypes_ListingTypeID",
                        column: x => x.ListingTypeID,
                        principalTable: "ListingTypes",
                        principalColumn: "ListingTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RenterID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    ListingTypeID = table.Column<int>(type: "int", nullable: true),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApproveBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetID);
                    table.ForeignKey(
                        name: "FK_Assets_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Assets_ListingTypes_ListingTypeID",
                        column: x => x.ListingTypeID,
                        principalTable: "ListingTypes",
                        principalColumn: "ListingTypeID");
                    table.ForeignKey(
                        name: "FK_Assets_Users_RenterID",
                        column: x => x.RenterID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchasedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RejectedReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchasedID);
                    table.ForeignKey(
                        name: "FK_Purchases_Assets_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Assets",
                        principalColumn: "AssetID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    PurchasedID = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Assets_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Assets",
                        principalColumn: "AssetID");
                    table.ForeignKey(
                        name: "FK_Payments_Purchases_PurchasedID",
                        column: x => x.PurchasedID,
                        principalTable: "Purchases",
                        principalColumn: "PurchasedID");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasedID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewerID);
                    table.ForeignKey(
                        name: "FK_Reviews_Payments_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Purchases_PurchasedID",
                        column: x => x.PurchasedID,
                        principalTable: "Purchases",
                        principalColumn: "PurchasedID");
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.InsertData(
                table: "ListingTypes",
                columns: new[] { "ListingTypeID", "CreatedAt", "Description", "IsActive", "TypeName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vehicles available for rent only", true, "Rent", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vehicles available for purchase only", true, "Sale", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vehicles available for both rent and purchase", true, "Both", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "CreatedAt", "Description", "IsActive", "RoleName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Full system access", true, "Admin", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Regular user - can view assets", true, "User", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Can list and manage assets", true, "Seller", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Can approve and moderate content", true, "Moderator", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName", "CreatedAt", "Description", "IsActive", "ListingTypeID", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Sedan", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standard 4-door passenger cars", true, 1, null },
                    { 2, "SUV", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sport Utility Vehicles - spacious and versatile", true, 1, null },
                    { 3, "Luxury", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium luxury vehicles for rent", true, 1, null },
                    { 4, "Motorcycle", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Two-wheeled motor vehicles for rent", true, 1, null },
                    { 5, "Van", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vans for passenger or cargo transport", true, 1, null },
                    { 6, "Electric", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electric vehicles (EVs) for rent", true, 1, null },
                    { 7, "Truck", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pickup trucks for hauling and heavy duty", true, 2, null },
                    { 8, "Classic", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic and vintage cars for sale", true, 2, null },
                    { 9, "Sports Car", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-performance sports cars", true, 2, null },
                    { 10, "Luxury", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium luxury vehicles for rent or purchase", true, 3, null },
                    { 11, "Electric", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electric vehicles (EVs) for rent or purchase", true, 3, null },
                    { 12, "Convertible", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Convertible/Open top vehicles", true, 3, null },
                    { 13, "SUV", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SUVs available for both rent and purchase", true, 3, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Address", "City", "CreatedAt", "Email", "Firstname", "IsActive", "Lastname", "PasswordHash", "PhoneNumber", "RoleID", "State", "Token", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@vehiclemarketplace.com", "System", true, "Administrator", "QWRtaW4xMjMh", null, 1, null, null, null, "admin" },
                    { 2, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John", true, "User", "VXNlcjEyMyE=", "123-456-7890", 2, null, null, null, "john_user" },
                    { 3, "123 Main St", "Los Angeles", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane@seller.com", "Jane", true, "Seller", "U2VsbGVyMTIzIQ==", "987-654-3210", 3, "CA", null, null, "jane_seller" },
                    { 4, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mod@moderator.com", "Mod", true, "Moderator", "TW9kMTIzIQ==", "555-555-5555", 4, null, null, null, "moderator" }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "AssetID", "ApprovalStatus", "ApproveBy", "CategoryID", "CreatedAt", "DailyRate", "Description", "IsActive", "ListingTypeID", "Location", "RenterID", "SalePrice", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Approved", 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45.00m, "Well-maintained sedan with leather seats, navigation, and backup camera. Perfect for business trips or family outings.", true, 1, "Los Angeles, CA", 3, null, "Available", "2024 Toyota Camry", null },
                    { 2, "Approved", 1, 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Powerful pickup truck with towing package, 4x4 capability, and bed liner. Great for construction or outdoor work.", true, 2, "Los Angeles, CA", 3, 35000.00m, "Available", "2019 Ford F-150", null },
                    { 3, "Approved", 1, 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85.00m, "Electric vehicle with autopilot, panoramic roof, and premium sound system. Zero emissions, instant torque.", true, 3, "Los Angeles, CA", 3, 48000.00m, "Rented", "2023 Tesla Model 3", null },
                    { 4, "Pending", null, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.00m, "Off-road capable SUV with removable doors and roof. Perfect for adventure seekers.", true, 1, "Los Angeles, CA", 3, null, "Available", "2022 Jeep Wrangler", null },
                    { 5, "Approved", 1, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mid-engine sports car with 490 horsepower, premium audio, and performance exhaust.", true, 2, "Los Angeles, CA", 3, 68000.00m, "Available", "2021 Chevrolet Corvette", null },
                    { 6, "Approved", 1, 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55.00m, "Compact SUV with hybrid engine, spacious cargo area, and Honda Sensing safety features.", true, 3, "Los Angeles, CA", 3, 32000.00m, "Sold", "2024 Honda CR-V", null }
                });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "PurchasedID", "BookingID", "Date", "PurchasePrice", "RejectedReason", "UserID" },
                values: new object[] { 1, 3, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 48000.00m, null, 2 });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentID", "BookingID", "PaidAt", "PaymentMethod", "PurchasedID", "Status", "TransactionNumber" },
                values: new object[] { 1, 3, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", 1, "Completed", "TXN-2026-001" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewerID", "Comment", "CreatedAt", "PaymentID", "PurchasedID", "Rating", "UserID" },
                values: new object[] { 1, "Excellent car! Very clean and well-maintained. Will rent again.", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, 5, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CategoryID",
                table: "Assets",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ListingTypeID",
                table: "Assets",
                column: "ListingTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_RenterID",
                table: "Assets",
                column: "RenterID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ListingTypeID",
                table: "Categories",
                column: "ListingTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingID",
                table: "Payments",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PurchasedID",
                table: "Payments",
                column: "PurchasedID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BookingID",
                table: "Purchases",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserID",
                table: "Purchases",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PaymentID",
                table: "Reviews",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PurchasedID",
                table: "Reviews",
                column: "PurchasedID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ListingTypes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}

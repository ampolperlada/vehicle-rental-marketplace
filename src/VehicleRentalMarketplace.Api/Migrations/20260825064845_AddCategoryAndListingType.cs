using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalMarketplace.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndListingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "Assets");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListingTypeId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ListingTypes",
                columns: table => new
                {
                    ListingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTypes", x => x.ListingTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ListingTypeId",
                table: "Assets",
                column: "ListingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Categories_CategoryId",
                table: "Assets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_ListingTypes_ListingTypeId",
                table: "Assets",
                column: "ListingTypeId",
                principalTable: "ListingTypes",
                principalColumn: "ListingTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Categories_CategoryId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_ListingTypes_ListingTypeId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ListingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ListingTypeId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ListingTypeId",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

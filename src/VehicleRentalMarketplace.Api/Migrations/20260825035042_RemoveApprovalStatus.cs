using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalMarketplace.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApprovalStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Users_ApprovedBy",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ApprovedBy",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Assets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedBy",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ApprovedBy",
                table: "Assets",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Users_ApprovedBy",
                table: "Assets",
                column: "ApprovedBy",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

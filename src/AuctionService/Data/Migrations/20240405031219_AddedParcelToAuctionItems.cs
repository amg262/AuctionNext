using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedParcelToAuctionItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Items",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Items",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Items",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "Items",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Items");
        }
    }
}

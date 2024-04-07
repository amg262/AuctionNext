using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedParcelStuffForShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Payments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Payments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Payments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "Payments",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Payments");
        }
    }
}

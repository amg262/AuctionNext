using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyToReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentId",
                table: "Rewards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_PaymentId",
                table: "Rewards",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Payments_PaymentId",
                table: "Rewards",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Payments_PaymentId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_PaymentId",
                table: "Rewards");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentId",
                table: "Rewards",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}

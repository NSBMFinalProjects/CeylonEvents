using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class updedtikets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 28, 47, 809, DateTimeKind.Local).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 28, 47, 809, DateTimeKind.Local).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 28, 47, 809, DateTimeKind.Local).AddTicks(8694));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 25, 11, 359, DateTimeKind.Local).AddTicks(6672));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 25, 11, 359, DateTimeKind.Local).AddTicks(6691));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 10, 25, 11, 359, DateTimeKind.Local).AddTicks(6693));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class removeenettype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 18, 40, 35, 520, DateTimeKind.Local).AddTicks(4000));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 18, 40, 35, 520, DateTimeKind.Local).AddTicks(4017));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 18, 40, 35, 520, DateTimeKind.Local).AddTicks(4020));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventType",
                value: "Conference");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventType",
                value: "Workshop");

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
    }
}

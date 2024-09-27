using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class upded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Events");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Slug", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8416), "tech-conference-2024", new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8425) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Slug", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8427), "design-workshop-2024", new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8428) });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8449));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8451));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8453));
        }
    }
}

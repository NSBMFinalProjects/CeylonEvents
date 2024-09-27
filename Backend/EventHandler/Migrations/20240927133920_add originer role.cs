using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class addoriginerrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NIC", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "reqId" },
                values: new object[] { "organiser-id-001", 0, "a891df8f-9057-4671-9cda-6a4615a44706", "organiser@example.com", true, "John", "Doe", false, null, "123456789321456", "ORGANISER@EXAMPLE.COM", "ORGANISER", "AQAAAAIAAYagAAAAEEAmjkidswNQUyRxhhX+tdI0YdFPLtk7u+j8E9+Lvh2eSdc8C8sZ9UwVJ+Hi2bl2Xw==", null, false, "e1fc2a41-3d5e-4a27-985d-a9989fa6f7cc", false, "organiser", 0 });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 19, 9, 19, 901, DateTimeKind.Local).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 19, 9, 19, 901, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "PurchaseDate",
                value: new DateTime(2024, 9, 27, 19, 9, 19, 901, DateTimeKind.Local).AddTicks(5441));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3", "organiser-id-001" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "organiser-id-001" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "organiser-id-001");

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
    }
}

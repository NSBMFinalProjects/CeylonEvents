using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class seedfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorys",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Conference", "conference" },
                    { 2, "WorkShop", "workshop" },
                    { 3, "Concert", "concert" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "EndDate", "EventName", "EventType", "Location", "OrganizerId", "Slug", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8416), "A conference for tech enthusiasts.", new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tech Conference 2024", "Conference", "New York", "acb56586-313a-423c-9027-55b8d1f04c4e", "tech-conference-2024", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8425) },
                    { 2, 2, new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8427), "A workshop for aspiring designers.", new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Design Workshop", "Workshop", "San Francisco", "acb56586-313a-423c-9027-55b8d1f04c4e", "design-workshop-2024", new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8428) }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "EventId", "Price", "PurchaseDate", "Quantity", "Status", "TicketName", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 299.99m, new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8449), 1, "Purchased", "VIP Pass", "42259ee0-748e-4bb6-9601-17bd762abdbf" },
                    { 2, 1, 99.99m, new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8451), 2, "Purchased", "General Admission", "42259ee0-748e-4bb6-9601-17bd762abdbf" },
                    { 3, 2, 199.99m, new DateTime(2024, 9, 25, 22, 27, 16, 541, DateTimeKind.Local).AddTicks(8453), 1, "Purchased", "Workshop Ticket", "42259ee0-748e-4bb6-9601-17bd762abdbf" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

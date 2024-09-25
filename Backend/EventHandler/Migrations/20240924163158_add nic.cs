using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHandler.Migrations
{
    /// <inheritdoc />
    public partial class addnic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NIC",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIC",
                table: "AspNetUsers");
        }
    }
}

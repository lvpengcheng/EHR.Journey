using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee.Migrations
{
    /// <inheritdoc />
    public partial class _20240130update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtensionData",
                table: "Employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtensionData",
                table: "Employees");
        }
    }
}

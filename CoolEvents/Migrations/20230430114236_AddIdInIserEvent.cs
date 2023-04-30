using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolEvents.Migrations
{
    /// <inheritdoc />
    public partial class AddIdInIserEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserEvents");
        }
    }
}

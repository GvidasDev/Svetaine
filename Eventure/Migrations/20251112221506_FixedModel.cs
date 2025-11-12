using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventure.Migrations
{
    /// <inheritdoc />
    public partial class FixedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Events",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}

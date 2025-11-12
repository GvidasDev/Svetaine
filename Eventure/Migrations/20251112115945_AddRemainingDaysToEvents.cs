using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingDaysToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingDays",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingDays",
                table: "Events");
        }
    }
}

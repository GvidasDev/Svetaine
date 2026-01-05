using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventure.Migrations
{
    /// <inheritdoc />
    public partial class TaskStatePerUserDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TaskStatuses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TaskStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskStatuses");
        }
    }
}

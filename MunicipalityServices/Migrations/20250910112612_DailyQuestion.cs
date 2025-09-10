using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalityServices.Migrations
{
    /// <inheritdoc />
    public partial class DailyQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PulseCompletedCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PulseCompletedCount",
                table: "Users");
        }
    }
}

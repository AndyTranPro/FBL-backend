using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FooBooLooGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Max",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Min",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Max",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Min",
                table: "Games");
        }
    }
}

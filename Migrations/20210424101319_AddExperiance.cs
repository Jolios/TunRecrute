using Microsoft.EntityFrameworkCore.Migrations;

namespace Tunrecrute.Migrations
{
    public partial class AddExperiance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Experiance",
                table: "Advertisements",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experiance",
                table: "Advertisements");
        }
    }
}

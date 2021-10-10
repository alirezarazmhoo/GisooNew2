using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class q3d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identifiecode",
                table: "Users",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "identifiecodeOwner",
                table: "Users",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "identifiecode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "identifiecodeOwner",
                table: "Users");
        }
    }
}

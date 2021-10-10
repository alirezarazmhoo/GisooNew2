using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class dfewr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "adminConfirmStatus",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "adminConfirmStatus",
                table: "ClassRooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adminConfirmStatus",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "adminConfirmStatus",
                table: "ClassRooms");
        }
    }
}

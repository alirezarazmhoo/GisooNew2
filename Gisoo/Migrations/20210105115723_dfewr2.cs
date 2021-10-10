using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class dfewr2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "countView",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countView",
                table: "Notices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countView",
                table: "Lines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countView",
                table: "ClassRooms",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countView",
                table: "Advertisments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "countView",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "countView",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "countView",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "countView",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "countView",
                table: "Advertisments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class wd524 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "discountPrice",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "discountPrice",
                table: "ClassRooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discountPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "discountPrice",
                table: "ClassRooms");
        }
    }
}

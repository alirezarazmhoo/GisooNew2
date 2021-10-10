using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class swg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Advertisments",
                newName: "image3");

            migrationBuilder.AddColumn<string>(
                name: "image1",
                table: "Advertisments",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image2",
                table: "Advertisments",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image1",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "image2",
                table: "Advertisments");

            migrationBuilder.RenameColumn(
                name: "image3",
                table: "Advertisments",
                newName: "image");
        }
    }
}

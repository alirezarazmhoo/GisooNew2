using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class swdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Notices",
                newName: "image3");

            migrationBuilder.AddColumn<string>(
                name: "image1",
                table: "Notices",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image2",
                table: "Notices",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image1",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "image2",
                table: "Notices");

            migrationBuilder.RenameColumn(
                name: "image3",
                table: "Notices",
                newName: "image");
        }
    }
}

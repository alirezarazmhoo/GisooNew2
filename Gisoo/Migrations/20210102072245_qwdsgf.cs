using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class qwdsgf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lineLaw",
                table: "ClassRooms");

            migrationBuilder.AddColumn<int>(
                name: "classRoomLaw",
                table: "ClassRooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "classRoomLaw",
                table: "ClassRooms");

            migrationBuilder.AddColumn<int>(
                name: "lineLaw",
                table: "ClassRooms",
                nullable: false,
                defaultValue: 0);
        }
    }
}

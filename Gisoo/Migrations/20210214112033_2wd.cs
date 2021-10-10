using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class _2wd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "classRoomPeriod",
                table: "ClassRooms",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "classRoomTeacher",
                table: "ClassRooms",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "classRoomPeriod",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "classRoomTeacher",
                table: "ClassRooms");
        }
    }
}

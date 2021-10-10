using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class w32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineWeekDateId",
                table: "Reserves",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LineWeekDateId",
                table: "Factors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineWeekDateId",
                table: "Reserves");

            migrationBuilder.DropColumn(
                name: "LineWeekDateId",
                table: "Factors");
        }
    }
}

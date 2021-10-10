using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class _4564 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nationalCode",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "score",
                table: "Users",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "fullname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "nationalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "score",
                table: "Users");
        }
    }
}

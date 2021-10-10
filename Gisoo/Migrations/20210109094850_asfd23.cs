using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class asfd23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "linkInstagram",
                table: "Users",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "linkTelegram",
                table: "Users",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shebaNumber",
                table: "Users",
                maxLength: 16,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "linkInstagram",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "linkTelegram",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shebaNumber",
                table: "Users");
        }
    }
}

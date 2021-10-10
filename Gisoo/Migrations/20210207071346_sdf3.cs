using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sdf3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isBuyOneMonth",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBuyOneMonth",
                table: "Users");
        }
    }
}

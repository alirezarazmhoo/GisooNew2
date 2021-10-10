using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "Users",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "Users",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6,
                oldNullable: true);
        }
    }
}

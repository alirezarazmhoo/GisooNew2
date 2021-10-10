using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class _7774 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "nationalCode",
                table: "Users",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userStatus",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userStatus",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "nationalCode",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}

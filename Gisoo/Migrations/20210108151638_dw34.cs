using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class dw34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "workingHours",
                table: "Users",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "shortDescription",
                table: "Users",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "longDescription",
                table: "Users",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Users",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "urlDocument",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "urlDocument",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "workingHours",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "shortDescription",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "longDescription",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class dw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdminConfirm",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "isAdminConfirm",
                table: "Advertisments");

            migrationBuilder.AddColumn<int>(
                name: "adminConfirmStatus",
                table: "Notices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notConfirmDescription",
                table: "Notices",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "adminConfirmStatus",
                table: "Advertisments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notConfirmDescription",
                table: "Advertisments",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adminConfirmStatus",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "notConfirmDescription",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "adminConfirmStatus",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "notConfirmDescription",
                table: "Advertisments");

            migrationBuilder.AddColumn<bool>(
                name: "isAdminConfirm",
                table: "Notices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isAdminConfirm",
                table: "Advertisments",
                nullable: false,
                defaultValue: false);
        }
    }
}

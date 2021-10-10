using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial5423534 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hasCertificate",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "latitude",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "longDescription",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "longitude",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "shortDescription",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workingHours",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "hasCertificate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "longDescription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shortDescription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "url",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "workingHours",
                table: "Users");
        }
    }
}

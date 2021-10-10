using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sdrwer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "reserveDate",
                table: "Lines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reserveHour",
                table: "Lines",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reserveDate",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "reserveHour",
                table: "Lines");
        }
    }
}

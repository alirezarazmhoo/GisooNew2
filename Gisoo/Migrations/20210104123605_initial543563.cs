using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial543563 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "longitude",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "latitude",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "longitude",
                table: "Users",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "latitude",
                table: "Users",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class dw3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "maxDiscount",
                table: "AllPrices",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "minDiscount",
                table: "AllPrices",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxDiscount",
                table: "AllPrices");

            migrationBuilder.DropColumn(
                name: "minDiscount",
                table: "AllPrices");
        }
    }
}

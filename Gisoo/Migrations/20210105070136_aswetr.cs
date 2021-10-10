using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class aswetr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "oneSubscriptionPrice",
                table: "AllPrices",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "threeSubscriptionPrice",
                table: "AllPrices",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oneSubscriptionPrice",
                table: "AllPrices");

            migrationBuilder.DropColumn(
                name: "threeSubscriptionPrice",
                table: "AllPrices");
        }
    }
}

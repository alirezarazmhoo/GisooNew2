using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial423345435 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Reserves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_productId",
                table: "Reserves",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves",
                column: "productId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves");

            migrationBuilder.DropIndex(
                name: "IX_Reserves_productId",
                table: "Reserves");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Reserves");
        }
    }
}

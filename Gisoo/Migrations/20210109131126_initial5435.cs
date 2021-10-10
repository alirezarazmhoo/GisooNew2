using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial5435 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves");

            migrationBuilder.AlterColumn<int>(
                name: "productId",
                table: "Reserves",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves",
                column: "productId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves");

            migrationBuilder.AlterColumn<int>(
                name: "productId",
                table: "Reserves",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_Products_productId",
                table: "Reserves",
                column: "productId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

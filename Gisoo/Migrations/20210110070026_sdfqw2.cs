using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sdfqw2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expireDate",
                table: "Lines",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "classRoomId",
                table: "Factors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "lineId",
                table: "Factors",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Factors",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "scoreWithInturducer",
                table: "AllPrices",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Factors_classRoomId",
                table: "Factors",
                column: "classRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_lineId",
                table: "Factors",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_productId",
                table: "Factors",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_ClassRooms_classRoomId",
                table: "Factors",
                column: "classRoomId",
                principalTable: "ClassRooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Lines_lineId",
                table: "Factors",
                column: "lineId",
                principalTable: "Lines",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Products_productId",
                table: "Factors",
                column: "productId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_ClassRooms_classRoomId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Lines_lineId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Products_productId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_classRoomId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_lineId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_productId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "expireDate",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "classRoomId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "lineId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Factors");

            migrationBuilder.AlterColumn<int>(
                name: "scoreWithInturducer",
                table: "AllPrices",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

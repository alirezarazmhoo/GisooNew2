using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class xcv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LineWeekDateId",
                table: "Reserves",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_LineWeekDateId",
                table: "Reserves",
                column: "LineWeekDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_LineWeekDates_LineWeekDateId",
                table: "Reserves",
                column: "LineWeekDateId",
                principalTable: "LineWeekDates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_LineWeekDates_LineWeekDateId",
                table: "Reserves");

            migrationBuilder.DropIndex(
                name: "IX_Reserves_LineWeekDateId",
                table: "Reserves");

            migrationBuilder.AlterColumn<string>(
                name: "LineWeekDateId",
                table: "Reserves",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

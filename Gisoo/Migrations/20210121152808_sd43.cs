using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sd43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_Users_userId",
                table: "Reserves");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Reserves",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userIdNoticeOwner",
                table: "Reserves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_Users_userId",
                table: "Reserves",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserves_Users_userId",
                table: "Reserves");

            migrationBuilder.DropColumn(
                name: "userIdNoticeOwner",
                table: "Reserves");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Reserves",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Reserves_Users_userId",
                table: "Reserves",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

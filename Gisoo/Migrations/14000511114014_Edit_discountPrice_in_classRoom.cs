using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class Edit_discountPrice_in_classRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "discountPrice",
                table: "ClassRooms",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "discountPrice",
                table: "ClassRooms",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}

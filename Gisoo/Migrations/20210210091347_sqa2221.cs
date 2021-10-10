using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class sqa2221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineWeekDates",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    lineId = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    fromTime = table.Column<string>(maxLength: 20, nullable: false),
                    toTime = table.Column<string>(maxLength: 20, nullable: false),
                    isReserved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineWeekDates", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineWeekDates_Lines_lineId",
                        column: x => x.lineId,
                        principalTable: "Lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineWeekDates_lineId",
                table: "LineWeekDates",
                column: "lineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineWeekDates");
        }
    }
}

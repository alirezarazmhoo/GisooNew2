using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial66456 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
 

            migrationBuilder.CreateTable(
                name: "ReservedSalowns",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: true),
                    price = table.Column<long>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    lineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedSalowns", x => x.id);
                    table.ForeignKey(
                        name: "FK_ReservedSalowns_Lines_lineId",
                        column: x => x.lineId,
                        principalTable: "Lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservedSalowns_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSalowns_lineId",
                table: "ReservedSalowns",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSalowns_userId",
                table: "ReservedSalowns",
                column: "userId");

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUss");

            migrationBuilder.DropTable(
                name: "AllPrices");

            migrationBuilder.DropTable(
                name: "AndroidVersions");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "ContactUss");

            migrationBuilder.DropTable(
                name: "FactorItems");

            migrationBuilder.DropTable(
                name: "Informations");

            migrationBuilder.DropTable(
                name: "LineImages");

            migrationBuilder.DropTable(
                name: "ReservedSalowns");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Advertisments");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "LineTypes");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}

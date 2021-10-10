using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class qg4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    price = table.Column<long>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    lineTypeId = table.Column<int>(nullable: false),
                    lineLaw = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lines_LineTypes_lineTypeId",
                        column: x => x.lineTypeId,
                        principalTable: "LineTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lines_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineImages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(maxLength: 200, nullable: true),
                    lineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_LineImages_Lines_lineId",
                        column: x => x.lineId,
                        principalTable: "Lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineImages_lineId",
                table: "LineImages",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_lineTypeId",
                table: "Lines",
                column: "lineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_userId",
                table: "Lines",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineImages");

            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}

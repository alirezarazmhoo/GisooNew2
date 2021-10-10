using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initial6645543 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedSalowns");

            migrationBuilder.CreateTable(
                name: "Reserves",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: true),
                    price = table.Column<long>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    lineId = table.Column<int>(nullable: true),
                    classroomId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reserves_ClassRooms_classroomId",
                        column: x => x.classroomId,
                        principalTable: "ClassRooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserves_Lines_lineId",
                        column: x => x.lineId,
                        principalTable: "Lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserves_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_classroomId",
                table: "Reserves",
                column: "classroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_lineId",
                table: "Reserves",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_userId",
                table: "Reserves",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserves");

            migrationBuilder.CreateTable(
                name: "ReservedSalowns",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(nullable: false),
                    lineId = table.Column<int>(nullable: true),
                    price = table.Column<long>(nullable: false),
                    userId = table.Column<int>(nullable: true)
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
    }
}

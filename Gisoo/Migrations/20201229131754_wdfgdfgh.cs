using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class wdfgdfgh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassRoomTypes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoomTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    price = table.Column<long>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    classRoomTypeId = table.Column<int>(nullable: false),
                    lineLaw = table.Column<int>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false),
                    reserveDate = table.Column<DateTime>(nullable: true),
                    reserveHour = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClassRooms_ClassRoomTypes_classRoomTypeId",
                        column: x => x.classRoomTypeId,
                        principalTable: "ClassRoomTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassRooms_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassRoomImages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(maxLength: 200, nullable: true),
                    classRoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoomImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClassRoomImages_ClassRooms_classRoomId",
                        column: x => x.classRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoomImages_classRoomId",
                table: "ClassRoomImages",
                column: "classRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_classRoomTypeId",
                table: "ClassRooms",
                column: "classRoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_userId",
                table: "ClassRooms",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassRoomImages");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "ClassRoomTypes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class asfd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "urlDocument",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserDocumentImages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(maxLength: 200, nullable: true),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocumentImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserDocumentImages_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDocumentImages_userId",
                table: "UserDocumentImages",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDocumentImages");

            migrationBuilder.AddColumn<string>(
                name: "urlDocument",
                table: "Users",
                nullable: true);
        }
    }
}

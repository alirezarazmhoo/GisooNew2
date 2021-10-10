using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class qd4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllSearchDetailViewModels",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    image1 = table.Column<string>(nullable: true),
                    userId = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllSearchDetailViewModels", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllSearchDetailViewModels");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class qsdaf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUss",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUss", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AllPrices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    barberPrice = table.Column<long>(nullable: false),
                    workshopPrice = table.Column<long>(nullable: false),
                    advertismentPrice = table.Column<long>(nullable: false),
                    saloonPrice = table.Column<long>(nullable: false),
                    isHasSaloonPrice = table.Column<bool>(nullable: false),
                    isHasAdvertismentPrice = table.Column<bool>(nullable: false),
                    isHasWorkshopPrice = table.Column<bool>(nullable: false),
                    isHasBarberPrice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPrices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUss",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    phone = table.Column<string>(maxLength: 50, nullable: true),
                    pageTelegramUrl = table.Column<string>(maxLength: 200, nullable: true),
                    pageInstagramUrl = table.Column<string>(maxLength: 200, nullable: true),
                    pageTwitterUrl = table.Column<string>(maxLength: 200, nullable: true),
                    email = table.Column<string>(maxLength: 200, nullable: true),
                    androidVersion = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUss", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Informations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 500, nullable: false),
                    description = table.Column<string>(maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Informations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleNameFa = table.Column<string>(maxLength: 1000, nullable: false),
                    RoleNameEn = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(maxLength: 500, nullable: true),
                    link = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    cityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.id);
                    table.ForeignKey(
                        name: "FK_Provinces_Cities_cityId",
                        column: x => x.cityId,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cellphone = table.Column<string>(maxLength: 11, nullable: false),
                    password = table.Column<string>(maxLength: 200, nullable: true),
                    passwordShow = table.Column<string>(maxLength: 20, nullable: true),
                    token = table.Column<string>(maxLength: 100, nullable: true),
                    roleId = table.Column<int>(nullable: false),
                    code = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    provinceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Areas_Provinces_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(maxLength: 500, nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    isAdminConfirm = table.Column<bool>(nullable: false),
                    isWorkshop = table.Column<bool>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    cityId = table.Column<int>(nullable: false),
                    provinceId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    createDate = table.Column<DateTime>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Advertisments_Areas_areaId",
                        column: x => x.areaId,
                        principalTable: "Areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisments_Cities_cityId",
                        column: x => x.cityId,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisments_Provinces_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisments_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(maxLength: 500, nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    condition = table.Column<int>(nullable: false),
                    isBarber = table.Column<bool>(nullable: false),
                    isAdminConfirm = table.Column<bool>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    cityId = table.Column<int>(nullable: false),
                    provinceId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    createDate = table.Column<DateTime>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notices_Areas_areaId",
                        column: x => x.areaId,
                        principalTable: "Areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notices_Cities_cityId",
                        column: x => x.cityId,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notices_Provinces_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notices_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: false),
                    advertismentId = table.Column<int>(nullable: true),
                    noticeId = table.Column<int>(nullable: true),
                    state = table.Column<int>(nullable: false),
                    createDatePersian = table.Column<string>(maxLength: 50, nullable: false),
                    totalPrice = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.id);
                    table.ForeignKey(
                        name: "FK_Factors_Advertisments_advertismentId",
                        column: x => x.advertismentId,
                        principalTable: "Advertisments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_Notices_noticeId",
                        column: x => x.noticeId,
                        principalTable: "Notices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactorItems",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    price = table.Column<long>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    FactorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_FactorItems_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactorItems_Notices_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Notices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_areaId",
                table: "Advertisments",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_cityId",
                table: "Advertisments",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_provinceId",
                table: "Advertisments",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_userId",
                table: "Advertisments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_provinceId",
                table: "Areas",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_FactorId",
                table: "FactorItems",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_ProductId",
                table: "FactorItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_advertismentId",
                table: "Factors",
                column: "advertismentId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_noticeId",
                table: "Factors",
                column: "noticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_userId",
                table: "Factors",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_areaId",
                table: "Notices",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_cityId",
                table: "Notices",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_provinceId",
                table: "Notices",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_userId",
                table: "Notices",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_cityId",
                table: "Provinces",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUss");

            migrationBuilder.DropTable(
                name: "AllPrices");

            migrationBuilder.DropTable(
                name: "ContactUss");

            migrationBuilder.DropTable(
                name: "FactorItems");

            migrationBuilder.DropTable(
                name: "Informations");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "Advertisments");

            migrationBuilder.DropTable(
                name: "Notices");

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

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class initFirst : Migration
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
                    threeSubscriptionPrice = table.Column<long>(nullable: false),
                    oneSubscriptionPrice = table.Column<long>(nullable: false),
                    isHasSaloonPrice = table.Column<bool>(nullable: false),
                    isHasAdvertismentPrice = table.Column<bool>(nullable: false),
                    isHasWorkshopPrice = table.Column<bool>(nullable: false),
                    isHasBarberPrice = table.Column<bool>(nullable: false),
                    minDiscount = table.Column<long>(nullable: false),
                    maxDiscount = table.Column<long>(nullable: false),
                    scoreWithInturducer = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPrices", x => x.id);
                });

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
                    tabletype = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllSearchDetailViewModels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AndroidVersions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    appAndroidUrl = table.Column<string>(maxLength: 70, nullable: true),
                    currVersion = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AndroidVersions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(maxLength: 500, nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(maxLength: 500, nullable: true),
                    link = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.id);
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
                name: "LineTypes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineTypes", x => x.id);
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
                    link = table.Column<string>(maxLength: 200, nullable: false),
                    titleForWebSite = table.Column<string>(maxLength: 200, nullable: true),
                    isForWebSite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    anyNoticeId = table.Column<int>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(maxLength: 50, nullable: false),
                    whichTableEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.id);
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
                    code = table.Column<string>(maxLength: 6, nullable: true),
                    nationalCode = table.Column<string>(maxLength: 10, nullable: true),
                    score = table.Column<long>(nullable: false),
                    fullname = table.Column<string>(maxLength: 200, nullable: true),
                    userStatus = table.Column<bool>(nullable: false),
                    url = table.Column<string>(nullable: true),
                    shortDescription = table.Column<string>(maxLength: 500, nullable: true),
                    longDescription = table.Column<string>(maxLength: 2000, nullable: true),
                    address = table.Column<string>(maxLength: 1000, nullable: true),
                    workingHours = table.Column<string>(maxLength: 1000, nullable: true),
                    hasCertificate = table.Column<bool>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    linkTelegram = table.Column<string>(maxLength: 100, nullable: true),
                    linkInstagram = table.Column<string>(maxLength: 100, nullable: true),
                    shebaNumber = table.Column<string>(maxLength: 16, nullable: true),
                    identifiecode = table.Column<string>(maxLength: 8, nullable: true),
                    identifiecodeOwner = table.Column<string>(maxLength: 8, nullable: true),
                    isscored = table.Column<bool>(nullable: false),
                    expireDateAccount = table.Column<DateTime>(nullable: true),
                    isProfileAccept = table.Column<bool>(nullable: false),
                    isProfileComplete = table.Column<bool>(nullable: false),
                    notConfirmDes = table.Column<string>(maxLength: 500, nullable: true),
                    isBuyOneMonth = table.Column<bool>(nullable: false),
                    sexuality = table.Column<bool>(nullable: false)
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
                    classRoomLaw = table.Column<int>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false),
                    reserveDate = table.Column<DateTime>(nullable: true),
                    reserveHour = table.Column<string>(maxLength: 50, nullable: true),
                    adminConfirmStatus = table.Column<int>(nullable: false),
                    countView = table.Column<int>(nullable: true),
                    expireDate = table.Column<DateTime>(nullable: false),
                    notConfirmDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    discountPrice = table.Column<long>(nullable: false),
                    classRoomTeacher = table.Column<string>(maxLength: 100, nullable: true),
                    classRoomPeriod = table.Column<string>(maxLength: 100, nullable: true)
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
                    lineLaw = table.Column<int>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false),
                    reserveDate = table.Column<DateTime>(nullable: true),
                    reserveHour = table.Column<string>(maxLength: 50, nullable: true),
                    adminConfirmStatus = table.Column<int>(nullable: false),
                    countView = table.Column<int>(nullable: true),
                    expireDate = table.Column<DateTime>(nullable: false),
                    notConfirmDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    discountPrice = table.Column<long>(nullable: true),
                    lineTeacher = table.Column<string>(maxLength: 100, nullable: true)
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
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    price = table.Column<long>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    adminConfirmStatus = table.Column<int>(nullable: false),
                    countView = table.Column<int>(nullable: true),
                    expireDate = table.Column<DateTime>(nullable: false),
                    notConfirmDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    discountPrice = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                    table.ForeignKey(
                        name: "FK_Products_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Advertisments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image1 = table.Column<string>(maxLength: 500, nullable: true),
                    image2 = table.Column<string>(maxLength: 500, nullable: true),
                    image3 = table.Column<string>(maxLength: 500, nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    notConfirmDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    adminConfirmStatus = table.Column<int>(nullable: false),
                    isWorkshop = table.Column<bool>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    cityId = table.Column<int>(nullable: false),
                    provinceId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    createDate = table.Column<DateTime>(maxLength: 50, nullable: false),
                    expireDate = table.Column<DateTime>(maxLength: 50, nullable: false),
                    isDeleted = table.Column<bool>(nullable: false),
                    countView = table.Column<int>(nullable: true)
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
                    image1 = table.Column<string>(maxLength: 500, nullable: true),
                    image2 = table.Column<string>(maxLength: 500, nullable: true),
                    image3 = table.Column<string>(maxLength: 500, nullable: true),
                    title = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    condition = table.Column<int>(nullable: false),
                    isBarber = table.Column<bool>(nullable: false),
                    notConfirmDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    adminConfirmStatus = table.Column<int>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    cityId = table.Column<int>(nullable: false),
                    provinceId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    createDate = table.Column<DateTime>(maxLength: 50, nullable: false),
                    expireDate = table.Column<DateTime>(maxLength: 50, nullable: false),
                    isDeleted = table.Column<bool>(nullable: false),
                    countView = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(maxLength: 200, nullable: true),
                    productId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
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
                    factorKind = table.Column<int>(nullable: false),
                    createDatePersian = table.Column<string>(maxLength: 50, nullable: false),
                    totalPrice = table.Column<long>(nullable: false),
                    lineId = table.Column<int>(nullable: true),
                    classRoomId = table.Column<int>(nullable: true),
                    productId = table.Column<int>(nullable: true),
                    LineWeekDateId = table.Column<string>(nullable: true)
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
                        name: "FK_Factors_ClassRooms_classRoomId",
                        column: x => x.classRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_Lines_lineId",
                        column: x => x.lineId,
                        principalTable: "Lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_Notices_noticeId",
                        column: x => x.noticeId,
                        principalTable: "Notices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
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
                name: "Reserves",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userIdNoticeOwner = table.Column<int>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    price = table.Column<long>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    lineId = table.Column<int>(nullable: true),
                    classroomId = table.Column<int>(nullable: true),
                    productId = table.Column<int>(nullable: true),
                    LineWeekDateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reserves_LineWeekDates_LineWeekDateId",
                        column: x => x.LineWeekDateId,
                        principalTable: "LineWeekDates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_Reserves_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserves_Users_userId",
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
                name: "IX_Factors_classRoomId",
                table: "Factors",
                column: "classRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_lineId",
                table: "Factors",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_noticeId",
                table: "Factors",
                column: "noticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_productId",
                table: "Factors",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_userId",
                table: "Factors",
                column: "userId");

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

            migrationBuilder.CreateIndex(
                name: "IX_LineWeekDates_lineId",
                table: "LineWeekDates",
                column: "lineId");

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
                name: "IX_ProductImages_productId",
                table: "ProductImages",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_userId",
                table: "Products",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_cityId",
                table: "Provinces",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_LineWeekDateId",
                table: "Reserves",
                column: "LineWeekDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_classroomId",
                table: "Reserves",
                column: "classroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_lineId",
                table: "Reserves",
                column: "lineId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_productId",
                table: "Reserves",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_userId",
                table: "Reserves",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocumentImages_userId",
                table: "UserDocumentImages",
                column: "userId");

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
                name: "AllSearchDetailViewModels");

            migrationBuilder.DropTable(
                name: "AndroidVersions");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "ClassRoomImages");

            migrationBuilder.DropTable(
                name: "ContactUss");

            migrationBuilder.DropTable(
                name: "FactorItems");

            migrationBuilder.DropTable(
                name: "Informations");

            migrationBuilder.DropTable(
                name: "LineImages");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Reserves");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "UserDocumentImages");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "LineWeekDates");

            migrationBuilder.DropTable(
                name: "Advertisments");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "ClassRoomTypes");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "LineTypes");

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

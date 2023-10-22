using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TraceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorCode = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TownShip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalityCode = table.Column<int>(type: "int", nullable: false),
                    SubLocality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<int>(type: "int", nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SideFloor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovSubsidy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Registered = table.Column<bool>(type: "bit", nullable: false),
                    Included = table.Column<bool>(type: "bit", nullable: false),
                    Apply = table.Column<bool>(type: "bit", nullable: false),
                    Decile = table.Column<int>(type: "int", nullable: false),
                    VerifyFamily = table.Column<bool>(type: "bit", nullable: false),
                    FamilyHeadNationalId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovSubsidy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonMeaning = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViolationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GovUserInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDateShamsi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShenasnamehNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthCertificatenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasSignPrivateKey = table.Column<bool>(type: "bit", nullable: false),
                    SubsidyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovUserInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovUserInfos_GovAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "GovAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GovUserInfos_GovSubsidy_SubsidyId",
                        column: x => x.SubsidyId,
                        principalTable: "GovSubsidy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "County",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_County", x => x.Id);
                    table.ForeignKey(
                        name: "FK_County_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GovFamily",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonNin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovUserInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovFamily_GovUserInfos_GovUserInfoId",
                        column: x => x.GovUserInfoId,
                        principalTable: "GovUserInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_County_CountyId",
                        column: x => x.CountyId,
                        principalTable: "County",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ParsimapCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShahrbinInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShahrbinInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShahrbinInstance_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActorRegion",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "int", nullable: false),
                    RegionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorRegion", x => new { x.ActorsId, x.RegionsId });
                    table.ForeignKey(
                        name: "FK_ActorRegion_Actor_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorRegion_Region_RegionsId",
                        column: x => x.RegionsId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_MediaType = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationLink_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Avatar_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_MediaType = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CitizenshipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address_RegionId = table.Column<int>(type: "int", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Valley = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Elevation = table.Column<double>(type: "float", nullable: true),
                    VerificationSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FcmToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flags = table.Column<int>(type: "int", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Region_Address_RegionId",
                        column: x => x.Address_RegionId,
                        principalTable: "Region",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Chart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chart_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image_MediaType = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutiveContractor",
                columns: table => new
                {
                    ExecutiveId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutiveContractor", x => new { x.ExecutiveId, x.ContractorId });
                    table.ForeignKey(
                        name: "FK_ExecutiveContractor_AspNetUsers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutiveContractor_AspNetUsers_ExecutiveId",
                        column: x => x.ExecutiveId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_Actor_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Poll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PollType = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poll_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Poll_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevisionUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RevisorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisionUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisionUnit_AspNetUsers_RevisorId",
                        column: x => x.RevisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleChart",
                columns: table => new
                {
                    ChartsId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleChart", x => new { x.ChartsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleChart_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleChart_Chart_ChartsId",
                        column: x => x.ChartsId,
                        principalTable: "Chart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnitOrganizationalUnit",
                columns: table => new
                {
                    OrganizationalUnitsId = table.Column<int>(type: "int", nullable: false),
                    ParentOrganizationalUnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnitOrganizationalUnit", x => new { x.OrganizationalUnitsId, x.ParentOrganizationalUnitsId });
                    table.ForeignKey(
                        name: "FK_OrganizationalUnitOrganizationalUnit_OrganizationalUnit_OrganizationalUnitsId",
                        column: x => x.OrganizationalUnitsId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnitOrganizationalUnit_OrganizationalUnit_ParentOrganizationalUnitsId",
                        column: x => x.ParentOrganizationalUnitsId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choice_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Poll_PollMedias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll_PollMedias", x => new { x.PollId, x.Id });
                    table.ForeignKey(
                        name: "FK_Poll_PollMedias_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevisionUnitId = table.Column<int>(type: "int", nullable: true),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Process_RevisionUnit_RevisionUnitId",
                        column: x => x.RevisionUnitId,
                        principalTable: "RevisionUnit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Process_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollAnswerPollChoice",
                columns: table => new
                {
                    AnswersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChoicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAnswerPollChoice", x => new { x.AnswersId, x.ChoicesId });
                    table.ForeignKey(
                        name: "FK_PollAnswerPollChoice_Answer_AnswersId",
                        column: x => x.AnswersId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PollAnswerPollChoice_Choice_ChoicesId",
                        column: x => x.ChoicesId,
                        principalTable: "Choice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ResponseDuration = table.Column<int>(type: "int", nullable: true),
                    CategoryType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ObjectionAllowed = table.Column<bool>(type: "bit", nullable: false),
                    EditingAllowed = table.Column<bool>(type: "bit", nullable: false),
                    HideMap = table.Column<bool>(type: "bit", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Category_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Category_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stage_AspNetRoles_DisplayRoleId",
                        column: x => x.DisplayRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stage_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormElementType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Default = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormElement_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuickAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Media_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Media_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_MediaType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuickAccess_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuickAccess_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActorProcessStage",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "int", nullable: false),
                    StagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorProcessStage", x => new { x.ActorsId, x.StagesId });
                    table.ForeignKey(
                        name: "FK_ActorProcessStage_Actor_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorProcessStage_Stage_StagesId",
                        column: x => x.StagesId,
                        principalTable: "Stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    ReportState = table.Column<int>(type: "int", nullable: false),
                    CanSendMessageToCitizen = table.Column<bool>(type: "bit", nullable: false),
                    Routine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransitionType = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsTransitionLogPublic = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transition_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transition_Stage_FromId",
                        column: x => x.FromId,
                        principalTable: "Stage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transition_Stage_ToId",
                        column: x => x.ToId,
                        principalTable: "Stage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BotActors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransitionId = table.Column<int>(type: "int", nullable: false),
                    MessageToCitizen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Visibility = table.Column<int>(type: "int", nullable: true),
                    ReasonId = table.Column<int>(type: "int", nullable: true),
                    ReasonMeaning = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotActors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotActors_Transition_TransitionId",
                        column: x => x.TransitionId,
                        principalTable: "Transition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessReasonProcessTransition",
                columns: table => new
                {
                    ReasonListId = table.Column<int>(type: "int", nullable: false),
                    TransitionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessReasonProcessTransition", x => new { x.ReasonListId, x.TransitionsId });
                    table.ForeignKey(
                        name: "FK_ProcessReasonProcessTransition_Reason_ReasonListId",
                        column: x => x.ReasonListId,
                        principalTable: "Reason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessReasonProcessTransition_Transition_TransitionsId",
                        column: x => x.TransitionsId,
                        principalTable: "Transition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false),
                    Sent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Finished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Responsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastStatusDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    ResponseDuration = table.Column<double>(type: "float", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Address_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address_RegionId = table.Column<int>(type: "int", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Valley = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Elevation = table.Column<double>(type: "float", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportState = table.Column<int>(type: "int", nullable: false),
                    LastStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    LastTransitionId = table.Column<int>(type: "int", nullable: true),
                    LastReasonId = table.Column<int>(type: "int", nullable: true),
                    CurrentActorsStr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CitizenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegistrantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExecutiveId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspectorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Flags = table.Column<int>(type: "int", nullable: false),
                    IsIdentityVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsObjectioned = table.Column<bool>(type: "bit", nullable: false),
                    IsFeedbacked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ExecutiveId",
                        column: x => x.ExecutiveId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_RegistrantId",
                        column: x => x.RegistrantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Reason_LastReasonId",
                        column: x => x.LastReasonId,
                        principalTable: "Reason",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Region_Address_RegionId",
                        column: x => x.Address_RegionId,
                        principalTable: "Region",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Stage_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "Stage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Transition_LastTransitionId",
                        column: x => x.LastTransitionId,
                        principalTable: "Transition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActorBotActor",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "int", nullable: false),
                    BotActorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorBotActor", x => new { x.ActorsId, x.BotActorsId });
                    table.ForeignKey(
                        name: "FK_ActorBotActor_Actor_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorBotActor_BotActors_BotActorsId",
                        column: x => x.BotActorsId,
                        principalTable: "BotActors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActorReport",
                columns: table => new
                {
                    CurrentActorsId = table.Column<int>(type: "int", nullable: false),
                    ReportsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorReport", x => new { x.CurrentActorsId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_ActorReport_Actor_CurrentActorsId",
                        column: x => x.CurrentActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorReport_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false),
                    ReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TryCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedback_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedback_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportLikes",
                columns: table => new
                {
                    LikedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportsLikedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLikes", x => new { x.LikedById, x.ReportsLikedId });
                    table.ForeignKey(
                        name: "FK_ReportLikes_AspNetUsers_LikedById",
                        column: x => x.LikedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportLikes_Reports_ReportsLikedId",
                        column: x => x.ReportsLikedId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports_Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports_Medias", x => new { x.ReportId, x.Id });
                    table.ForeignKey(
                        name: "FK_Reports_Medias_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransitionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportLogType = table.Column<int>(type: "int", nullable: false),
                    TransitionId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: true),
                    ActorType = table.Column<int>(type: "int", nullable: false),
                    ActorIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransitionLogs_Reason_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "Reason",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransitionLogs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransitionLogs_Transition_TransitionId",
                        column: x => x.TransitionId,
                        principalTable: "Transition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Violation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViolationTypeId = table.Column<int>(type: "int", nullable: false),
                    ViolationCheckResult = table.Column<int>(type: "int", nullable: true),
                    ViolatoinCheckDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShahrbinInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Violation_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Violation_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Violation_ShahrbinInstance_ShahrbinInstanceId",
                        column: x => x.ShahrbinInstanceId,
                        principalTable: "ShahrbinInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Violation_ViolationType_ViolationTypeId",
                        column: x => x.ViolationTypeId,
                        principalTable: "ViolationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageRecepient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRecepient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransitionLogs_Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransitionLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionLogs_Attachments", x => new { x.TransitionLogId, x.Id });
                    table.ForeignKey(
                        name: "FK_TransitionLogs_Attachments_TransitionLogs_TransitionLogId",
                        column: x => x.TransitionLogId,
                        principalTable: "TransitionLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actor_Identifier",
                table: "Actor",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActorBotActor_BotActorsId",
                table: "ActorBotActor",
                column: "BotActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorProcessStage_StagesId",
                table: "ActorProcessStage",
                column: "StagesId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorRegion_RegionsId",
                table: "ActorRegion",
                column: "RegionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorReport_ReportsId",
                table: "ActorReport",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_PollId",
                table: "Answer",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLink_ShahrbinInstanceId",
                table: "ApplicationLink",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleChart_RolesId",
                table: "ApplicationRoleChart",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Address_RegionId",
                table: "AspNetUsers",
                column: "Address_RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShahrbinInstanceId",
                table: "AspNetUsers",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BotActors_TransitionId",
                table: "BotActors",
                column: "TransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentId",
                table: "Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ProcessId",
                table: "Category",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ShahrbinInstanceId",
                table: "Category",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Chart_ShahrbinInstanceId",
                table: "Chart",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_PollId",
                table: "Choice",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_City_DistrictId",
                table: "City",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReplyId",
                table: "Comment",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReportId",
                table: "Comment",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ShahrbinInstanceId",
                table: "Comment",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_County_ProvinceId",
                table: "County",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_District_CountyId",
                table: "District",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutiveContractor_ContractorId",
                table: "ExecutiveContractor",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ReportId",
                table: "Feedback",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ShahrbinInstanceId",
                table: "Feedback",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormElement_CategoryId",
                table: "FormElement",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GovFamily_GovUserInfoId",
                table: "GovFamily",
                column: "GovUserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GovUserInfos_AddressId",
                table: "GovUserInfos",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_GovUserInfos_SubsidyId",
                table: "GovUserInfos",
                column: "SubsidyId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromId",
                table: "Message",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReportId",
                table: "Message",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ShahrbinInstanceId",
                table: "Message",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_MessageId",
                table: "MessageRecepient",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_News_ShahrbinInstanceId",
                table: "News",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_ActorId",
                table: "OrganizationalUnit",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_ShahrbinInstanceId",
                table: "OrganizationalUnit",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_UserId",
                table: "OrganizationalUnit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnitOrganizationalUnit_ParentOrganizationalUnitsId",
                table: "OrganizationalUnitOrganizationalUnit",
                column: "ParentOrganizationalUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_AuthorId",
                table: "Poll",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_ShahrbinInstanceId",
                table: "Poll",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PollAnswerPollChoice_ChoicesId",
                table: "PollAnswerPollChoice",
                column: "ChoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_RevisionUnitId",
                table: "Process",
                column: "RevisionUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_ShahrbinInstanceId",
                table: "Process",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessReasonProcessTransition_TransitionsId",
                table: "ProcessReasonProcessTransition",
                column: "TransitionsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickAccess_CategoryId",
                table: "QuickAccess",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickAccess_ShahrbinInstanceId",
                table: "QuickAccess",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CityId",
                table: "Region",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLikes_ReportsLikedId",
                table: "ReportLikes",
                column: "ReportsLikedId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Address_RegionId",
                table: "Reports",
                column: "Address_RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CategoryId",
                table: "Reports",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CitizenId",
                table: "Reports",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ContractorId",
                table: "Reports",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CurrentStageId",
                table: "Reports",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ExecutiveId",
                table: "Reports",
                column: "ExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_InspectorId",
                table: "Reports",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LastReasonId",
                table: "Reports",
                column: "LastReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LastTransitionId",
                table: "Reports",
                column: "LastTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProcessId",
                table: "Reports",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RegistrantId",
                table: "Reports",
                column: "RegistrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ShahrbinInstanceId",
                table: "Reports",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_RevisionUnit_RevisorId",
                table: "RevisionUnit",
                column: "RevisorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShahrbinInstance_CityId",
                table: "ShahrbinInstance",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_DisplayRoleId",
                table: "Stage",
                column: "DisplayRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_ProcessId",
                table: "Stage",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_FromId",
                table: "Transition",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_ProcessId",
                table: "Transition",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Transition_ToId",
                table: "Transition",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionLogs_ReasonId",
                table: "TransitionLogs",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionLogs_ReportId",
                table: "TransitionLogs",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionLogs_TransitionId",
                table: "TransitionLogs",
                column: "TransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_CommentId",
                table: "Violation",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_ReportId",
                table: "Violation",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_ShahrbinInstanceId",
                table: "Violation",
                column: "ShahrbinInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_UserId",
                table: "Violation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_ViolationTypeId",
                table: "Violation",
                column: "ViolationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorBotActor");

            migrationBuilder.DropTable(
                name: "ActorProcessStage");

            migrationBuilder.DropTable(
                name: "ActorRegion");

            migrationBuilder.DropTable(
                name: "ActorReport");

            migrationBuilder.DropTable(
                name: "ApplicationLink");

            migrationBuilder.DropTable(
                name: "ApplicationRoleChart");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ExecutiveContractor");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "FormElement");

            migrationBuilder.DropTable(
                name: "GovFamily");

            migrationBuilder.DropTable(
                name: "MessageRecepient");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "OrganizationalUnitOrganizationalUnit");

            migrationBuilder.DropTable(
                name: "Poll_PollMedias");

            migrationBuilder.DropTable(
                name: "PollAnswerPollChoice");

            migrationBuilder.DropTable(
                name: "ProcessReasonProcessTransition");

            migrationBuilder.DropTable(
                name: "QuickAccess");

            migrationBuilder.DropTable(
                name: "ReportLikes");

            migrationBuilder.DropTable(
                name: "Reports_Medias");

            migrationBuilder.DropTable(
                name: "TransitionLogs_Attachments");

            migrationBuilder.DropTable(
                name: "Violation");

            migrationBuilder.DropTable(
                name: "BotActors");

            migrationBuilder.DropTable(
                name: "Chart");

            migrationBuilder.DropTable(
                name: "GovUserInfos");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "OrganizationalUnit");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "TransitionLogs");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "ViolationType");

            migrationBuilder.DropTable(
                name: "GovAddress");

            migrationBuilder.DropTable(
                name: "GovSubsidy");

            migrationBuilder.DropTable(
                name: "Actor");

            migrationBuilder.DropTable(
                name: "Poll");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Reason");

            migrationBuilder.DropTable(
                name: "Transition");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Process");

            migrationBuilder.DropTable(
                name: "RevisionUnit");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "ShahrbinInstance");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "County");

            migrationBuilder.DropTable(
                name: "Province");
        }
    }
}

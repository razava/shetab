using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MyYazdUserInfoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyYazdUserInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresIn = table.Column<int>(type: "int", nullable: true),
                    TokenType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Point = table.Column<int>(type: "int", nullable: true),
                    User_Balance = table.Column<int>(type: "int", nullable: true),
                    User_Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Gender = table.Column<int>(type: "int", nullable: true),
                    User_Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Tel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyYazdUserInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyYazdUserInfo");
        }
    }
}

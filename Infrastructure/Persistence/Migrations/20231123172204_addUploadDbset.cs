using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addUploadDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Upload",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Media_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Media_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_MediaType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upload_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Upload_UserId",
                table: "Upload",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Upload");
        }
    }
}

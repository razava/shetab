using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MediaRemovedFromPoll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poll_PollMedias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poll_PollMedias",
                columns: table => new
                {
                    PollId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
        }
    }
}

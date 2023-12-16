using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PollAnswerConfigurationModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer");

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer");

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

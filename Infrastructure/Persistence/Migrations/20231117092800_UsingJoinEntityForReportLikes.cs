using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UsingJoinEntityForReportLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportLikes_Reports_ReportsLikedId",
                table: "ReportLikes");

            migrationBuilder.RenameColumn(
                name: "ReportsLikedId",
                table: "ReportLikes",
                newName: "ReportId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportLikes_ReportsLikedId",
                table: "ReportLikes",
                newName: "IX_ReportLikes_ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportLikes_Reports_ReportId",
                table: "ReportLikes",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportLikes_Reports_ReportId",
                table: "ReportLikes");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "ReportLikes",
                newName: "ReportsLikedId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportLikes_ReportId",
                table: "ReportLikes",
                newName: "IX_ReportLikes_ReportsLikedId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportLikes_Reports_ReportsLikedId",
                table: "ReportLikes",
                column: "ReportsLikedId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

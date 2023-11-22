using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CurrentActorsChangedToCurrentActor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorReport");

            migrationBuilder.DropTable(
                name: "BotActorDestinationActors");

            migrationBuilder.DropColumn(
                name: "CurrentActorsStr",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "CurrentActorId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationActorId",
                table: "BotActors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CurrentActorId",
                table: "Reports",
                column: "CurrentActorId");

            migrationBuilder.CreateIndex(
                name: "IX_BotActors_DestinationActorId",
                table: "BotActors",
                column: "DestinationActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BotActors_Actor_DestinationActorId",
                table: "BotActors",
                column: "DestinationActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Actor_CurrentActorId",
                table: "Reports",
                column: "CurrentActorId",
                principalTable: "Actor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotActors_Actor_DestinationActorId",
                table: "BotActors");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Actor_CurrentActorId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CurrentActorId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_BotActors_DestinationActorId",
                table: "BotActors");

            migrationBuilder.DropColumn(
                name: "CurrentActorId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DestinationActorId",
                table: "BotActors");

            migrationBuilder.AddColumn<string>(
                name: "CurrentActorsStr",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "BotActorDestinationActors",
                columns: table => new
                {
                    BotActor1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationActorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotActorDestinationActors", x => new { x.BotActor1Id, x.DestinationActorsId });
                    table.ForeignKey(
                        name: "FK_BotActorDestinationActors_Actor_DestinationActorsId",
                        column: x => x.DestinationActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotActorDestinationActors_BotActors_BotActor1Id",
                        column: x => x.BotActor1Id,
                        principalTable: "BotActors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorReport_ReportsId",
                table: "ActorReport",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_BotActorDestinationActors_DestinationActorsId",
                table: "BotActorDestinationActors",
                column: "DestinationActorsId");
        }
    }
}

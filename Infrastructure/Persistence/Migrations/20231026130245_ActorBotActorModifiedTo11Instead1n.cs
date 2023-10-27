using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActorBotActorModifiedTo11Instead1n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Actor_BotActorId",
                table: "Actor");

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
                name: "IX_Actor_BotActorId",
                table: "Actor",
                column: "BotActorId",
                unique: true,
                filter: "[BotActorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BotActorDestinationActors_DestinationActorsId",
                table: "BotActorDestinationActors",
                column: "DestinationActorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotActorDestinationActors");

            migrationBuilder.DropIndex(
                name: "IX_Actor_BotActorId",
                table: "Actor");

            migrationBuilder.CreateIndex(
                name: "IX_Actor_BotActorId",
                table: "Actor",
                column: "BotActorId");
        }
    }
}

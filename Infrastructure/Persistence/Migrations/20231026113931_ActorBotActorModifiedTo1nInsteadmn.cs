using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActorBotActorModifiedTo1nInsteadmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorBotActor");

            migrationBuilder.AddColumn<string>(
                name: "BotActorId",
                table: "Actor",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actor_BotActorId",
                table: "Actor",
                column: "BotActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actor_BotActors_BotActorId",
                table: "Actor",
                column: "BotActorId",
                principalTable: "BotActors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actor_BotActors_BotActorId",
                table: "Actor");

            migrationBuilder.DropIndex(
                name: "IX_Actor_BotActorId",
                table: "Actor");

            migrationBuilder.DropColumn(
                name: "BotActorId",
                table: "Actor");

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

            migrationBuilder.CreateIndex(
                name: "IX_ActorBotActor_BotActorsId",
                table: "ActorBotActor",
                column: "BotActorsId");
        }
    }
}

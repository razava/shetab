using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InstanceIdAddedToForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShahrbinInstanceId",
                table: "Form",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Form_ShahrbinInstanceId",
                table: "Form",
                column: "ShahrbinInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_ShahrbinInstance_ShahrbinInstanceId",
                table: "Form",
                column: "ShahrbinInstanceId",
                principalTable: "ShahrbinInstance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Form_ShahrbinInstance_ShahrbinInstanceId",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Form_ShahrbinInstanceId",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "ShahrbinInstanceId",
                table: "Form");
        }
    }
}

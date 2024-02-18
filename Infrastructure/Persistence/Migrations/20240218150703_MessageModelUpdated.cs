using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MessageModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "Message",
                newName: "MessageSubject");

            migrationBuilder.RenameColumn(
                name: "LastSent",
                table: "Message",
                newName: "LastSentSms");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSentPush",
                table: "Message",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageSendingType",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSentPush",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageSendingType",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "MessageSubject",
                table: "Message",
                newName: "MessageType");

            migrationBuilder.RenameColumn(
                name: "LastSentSms",
                table: "Message",
                newName: "LastSent");
        }
    }
}

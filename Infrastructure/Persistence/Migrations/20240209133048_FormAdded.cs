using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FormAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormElement_Category_CategoryId",
                table: "FormElement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormElement",
                table: "FormElement");

            migrationBuilder.DropIndex(
                name: "IX_FormElement_CategoryId",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "Default",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "DefaultId",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "FormElementType",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "MaxLength",
                table: "FormElement");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "FormElement",
                newName: "Meta");

            migrationBuilder.RenameColumn(
                name: "Hint",
                table: "FormElement",
                newName: "ElementType");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "FormElement",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "FormId",
                table: "FormElement",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FormId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormElement",
                table: "FormElement",
                columns: new[] { "FormId", "Id" });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_FormId",
                table: "Category",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Form_FormId",
                table: "Category",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormElement_Form_FormId",
                table: "FormElement",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Form_FormId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_FormElement_Form_FormId",
                table: "FormElement");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormElement",
                table: "FormElement");

            migrationBuilder.DropIndex(
                name: "IX_Category_FormId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "FormElement");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "Meta",
                table: "FormElement",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "ElementType",
                table: "FormElement",
                newName: "Hint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FormElement",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "FormElement",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Default",
                table: "FormElement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DefaultId",
                table: "FormElement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormElementType",
                table: "FormElement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "FormElement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "FormElement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "FormElement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxLength",
                table: "FormElement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormElement",
                table: "FormElement",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FormElement_CategoryId",
                table: "FormElement",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormElement_Category_CategoryId",
                table: "FormElement",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}

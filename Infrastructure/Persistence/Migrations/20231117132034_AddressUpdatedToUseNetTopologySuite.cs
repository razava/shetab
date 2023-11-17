using Microsoft.EntityFrameworkCore.Migrations;
using Point = NetTopologySuite.Geometries.Point;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddressUpdatedToUseNetTopologySuite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Elevation",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Address_Longitude",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Address_Elevation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address_Longitude",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Point>(
                name: "Address_Location",
                table: "Reports",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Address_Location",
                table: "AspNetUsers",
                type: "geography",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Location",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Address_Location",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "Address_Elevation",
                table: "Reports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Latitude",
                table: "Reports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Longitude",
                table: "Reports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Elevation",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Latitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Longitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }
    }
}

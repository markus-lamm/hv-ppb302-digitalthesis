using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsVisible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "MolecularMosaics",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "MolarMosaics",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "GeoTags",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "MolecularMosaics");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "MolarMosaics");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "GeoTags");
        }
    }
}

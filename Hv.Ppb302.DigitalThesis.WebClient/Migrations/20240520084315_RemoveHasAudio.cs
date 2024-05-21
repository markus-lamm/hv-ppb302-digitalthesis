using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHasAudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAudio",
                table: "MolecularMosaics");

            migrationBuilder.DropColumn(
                name: "HasAudio",
                table: "MolarMosaics");

            migrationBuilder.DropColumn(
                name: "HasAudio",
                table: "GeoTags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAudio",
                table: "MolecularMosaics",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAudio",
                table: "MolarMosaics",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAudio",
                table: "GeoTags",
                type: "bit",
                nullable: true);
        }
    }
}

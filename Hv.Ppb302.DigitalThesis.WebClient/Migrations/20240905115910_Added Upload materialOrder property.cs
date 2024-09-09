using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddedUploadmaterialOrderproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialOrder",
                table: "Uploads",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialOrder",
                table: "Uploads");
        }
    }
}

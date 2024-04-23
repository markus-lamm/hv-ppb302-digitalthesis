using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MolarMosaics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolarMosaics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MolecularMosaics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolecularMosaics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MolarMosaicsTag",
                columns: table => new
                {
                    MolarMosaicsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolarMosaicsTag", x => new { x.MolarMosaicsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MolarMosaicsTag_MolarMosaics_MolarMosaicsId",
                        column: x => x.MolarMosaicsId,
                        principalTable: "MolarMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MolarMosaicsTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MolecularMosaicTag",
                columns: table => new
                {
                    MolecularMosaicsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolecularMosaicTag", x => new { x.MolecularMosaicsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MolecularMosaicTag_MolecularMosaics_MolecularMosaicsId",
                        column: x => x.MolecularMosaicsId,
                        principalTable: "MolecularMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MolecularMosaicTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MolarMosaicsTag_TagsId",
                table: "MolarMosaicsTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_MolecularMosaicTag_TagsId",
                table: "MolecularMosaicTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTag");

            migrationBuilder.DropTable(
                name: "MolarMosaicsTag");

            migrationBuilder.DropTable(
                name: "MolecularMosaicTag");

            migrationBuilder.DropTable(
                name: "MolarMosaics");

            migrationBuilder.DropTable(
                name: "MolecularMosaics");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}

using System;
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
                name: "GeoTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAudio = table.Column<bool>(type: "bit", nullable: true),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MolarMosaics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAudio = table.Column<bool>(type: "bit", nullable: true),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAudio = table.Column<bool>(type: "bit", nullable: true),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolecularMosaics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupTagMolarMosaic",
                columns: table => new
                {
                    GroupTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolarMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTagMolarMosaic", x => new { x.GroupTagsId, x.MolarMosaicsId });
                    table.ForeignKey(
                        name: "FK_GroupTagMolarMosaic_GroupTags_GroupTagsId",
                        column: x => x.GroupTagsId,
                        principalTable: "GroupTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTagMolarMosaic_MolarMosaics_MolarMosaicsId",
                        column: x => x.MolarMosaicsId,
                        principalTable: "MolarMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupTagMolecularMosaic",
                columns: table => new
                {
                    GroupTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolecularMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTagMolecularMosaic", x => new { x.GroupTagsId, x.MolecularMosaicsId });
                    table.ForeignKey(
                        name: "FK_GroupTagMolecularMosaic_GroupTags_GroupTagsId",
                        column: x => x.GroupTagsId,
                        principalTable: "GroupTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTagMolecularMosaic_MolecularMosaics_MolecularMosaicsId",
                        column: x => x.MolecularMosaicsId,
                        principalTable: "MolecularMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagMolarMosaic_MolarMosaicsId",
                table: "GroupTagMolarMosaic",
                column: "MolarMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagMolecularMosaic_MolecularMosaicsId",
                table: "GroupTagMolecularMosaic",
                column: "MolecularMosaicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTags");

            migrationBuilder.DropTable(
                name: "GroupTagMolarMosaic");

            migrationBuilder.DropTable(
                name: "GroupTagMolecularMosaic");

            migrationBuilder.DropTable(
                name: "MolarMosaics");

            migrationBuilder.DropTable(
                name: "GroupTags");

            migrationBuilder.DropTable(
                name: "MolecularMosaics");
        }
    }
}

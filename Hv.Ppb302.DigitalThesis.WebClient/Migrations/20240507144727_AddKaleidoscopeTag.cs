using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddKaleidoscopeTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorTagKaleidoscopeMosaic");

            migrationBuilder.DropTable(
                name: "KaleidoscopeMosaics");

            migrationBuilder.CreateTable(
                name: "KaleidoscopeTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaleidoscopeTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KaleidoscopeTagMolarMosaic",
                columns: table => new
                {
                    KaleidoscopeTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolarMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaleidoscopeTagMolarMosaic", x => new { x.KaleidoscopeTagsId, x.MolarMosaicsId });
                    table.ForeignKey(
                        name: "FK_KaleidoscopeTagMolarMosaic_KaleidoscopeTags_KaleidoscopeTagsId",
                        column: x => x.KaleidoscopeTagsId,
                        principalTable: "KaleidoscopeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KaleidoscopeTagMolarMosaic_MolarMosaics_MolarMosaicsId",
                        column: x => x.MolarMosaicsId,
                        principalTable: "MolarMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KaleidoscopeTagMolecularMosaic",
                columns: table => new
                {
                    KaleidoscopeTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolecularMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaleidoscopeTagMolecularMosaic", x => new { x.KaleidoscopeTagsId, x.MolecularMosaicsId });
                    table.ForeignKey(
                        name: "FK_KaleidoscopeTagMolecularMosaic_KaleidoscopeTags_KaleidoscopeTagsId",
                        column: x => x.KaleidoscopeTagsId,
                        principalTable: "KaleidoscopeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KaleidoscopeTagMolecularMosaic_MolecularMosaics_MolecularMosaicsId",
                        column: x => x.MolecularMosaicsId,
                        principalTable: "MolecularMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KaleidoscopeTagMolarMosaic_MolarMosaicsId",
                table: "KaleidoscopeTagMolarMosaic",
                column: "MolarMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_KaleidoscopeTagMolecularMosaic_MolecularMosaicsId",
                table: "KaleidoscopeTagMolecularMosaic",
                column: "MolecularMosaicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KaleidoscopeTagMolarMosaic");

            migrationBuilder.DropTable(
                name: "KaleidoscopeTagMolecularMosaic");

            migrationBuilder.DropTable(
                name: "KaleidoscopeTags");

            migrationBuilder.CreateTable(
                name: "KaleidoscopeMosaics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AudioFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAudio = table.Column<bool>(type: "bit", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaleidoscopeMosaics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectorTagKaleidoscopeMosaic",
                columns: table => new
                {
                    ConnectorTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KaleidoscopeMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorTagKaleidoscopeMosaic", x => new { x.ConnectorTagsId, x.KaleidoscopeMosaicsId });
                    table.ForeignKey(
                        name: "FK_ConnectorTagKaleidoscopeMosaic_ConnectorTags_ConnectorTagsId",
                        column: x => x.ConnectorTagsId,
                        principalTable: "ConnectorTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectorTagKaleidoscopeMosaic_KaleidoscopeMosaics_KaleidoscopeMosaicsId",
                        column: x => x.KaleidoscopeMosaicsId,
                        principalTable: "KaleidoscopeMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagKaleidoscopeMosaic_KaleidoscopeMosaicsId",
                table: "ConnectorTagKaleidoscopeMosaic",
                column: "KaleidoscopeMosaicsId");
        }
    }
}

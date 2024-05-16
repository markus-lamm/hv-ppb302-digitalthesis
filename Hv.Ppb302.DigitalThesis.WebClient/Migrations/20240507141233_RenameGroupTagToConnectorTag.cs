using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class RenameGroupTagToConnectorTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTagGroupTag");

            migrationBuilder.DropTable(
                name: "GroupTagKaleidoscopeMosaic");

            migrationBuilder.DropTable(
                name: "GroupTagMolarMosaic");

            migrationBuilder.DropTable(
                name: "GroupTagMolecularMosaic");

            migrationBuilder.DropTable(
                name: "GroupTags");

            migrationBuilder.CreateTable(
                name: "ConnectorTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectorTagGeoTag",
                columns: table => new
                {
                    ConnectorTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeoTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorTagGeoTag", x => new { x.ConnectorTagsId, x.GeoTagsId });
                    table.ForeignKey(
                        name: "FK_ConnectorTagGeoTag_ConnectorTags_ConnectorTagsId",
                        column: x => x.ConnectorTagsId,
                        principalTable: "ConnectorTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectorTagGeoTag_GeoTags_GeoTagsId",
                        column: x => x.GeoTagsId,
                        principalTable: "GeoTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ConnectorTagMolarMosaic",
                columns: table => new
                {
                    ConnectorTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolarMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorTagMolarMosaic", x => new { x.ConnectorTagsId, x.MolarMosaicsId });
                    table.ForeignKey(
                        name: "FK_ConnectorTagMolarMosaic_ConnectorTags_ConnectorTagsId",
                        column: x => x.ConnectorTagsId,
                        principalTable: "ConnectorTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectorTagMolarMosaic_MolarMosaics_MolarMosaicsId",
                        column: x => x.MolarMosaicsId,
                        principalTable: "MolarMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectorTagMolecularMosaic",
                columns: table => new
                {
                    ConnectorTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MolecularMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectorTagMolecularMosaic", x => new { x.ConnectorTagsId, x.MolecularMosaicsId });
                    table.ForeignKey(
                        name: "FK_ConnectorTagMolecularMosaic_ConnectorTags_ConnectorTagsId",
                        column: x => x.ConnectorTagsId,
                        principalTable: "ConnectorTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectorTagMolecularMosaic_MolecularMosaics_MolecularMosaicsId",
                        column: x => x.MolecularMosaicsId,
                        principalTable: "MolecularMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagGeoTag_GeoTagsId",
                table: "ConnectorTagGeoTag",
                column: "GeoTagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagKaleidoscopeMosaic_KaleidoscopeMosaicsId",
                table: "ConnectorTagKaleidoscopeMosaic",
                column: "KaleidoscopeMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagMolarMosaic_MolarMosaicsId",
                table: "ConnectorTagMolarMosaic",
                column: "MolarMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagMolecularMosaic_MolecularMosaicsId",
                table: "ConnectorTagMolecularMosaic",
                column: "MolecularMosaicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorTagGeoTag");

            migrationBuilder.DropTable(
                name: "ConnectorTagKaleidoscopeMosaic");

            migrationBuilder.DropTable(
                name: "ConnectorTagMolarMosaic");

            migrationBuilder.DropTable(
                name: "ConnectorTagMolecularMosaic");

            migrationBuilder.DropTable(
                name: "ConnectorTags");

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
                name: "GeoTagGroupTag",
                columns: table => new
                {
                    GeoTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoTagGroupTag", x => new { x.GeoTagsId, x.GroupTagsId });
                    table.ForeignKey(
                        name: "FK_GeoTagGroupTag_GeoTags_GeoTagsId",
                        column: x => x.GeoTagsId,
                        principalTable: "GeoTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoTagGroupTag_GroupTags_GroupTagsId",
                        column: x => x.GroupTagsId,
                        principalTable: "GroupTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupTagKaleidoscopeMosaic",
                columns: table => new
                {
                    GroupTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KaleidoscopeMosaicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTagKaleidoscopeMosaic", x => new { x.GroupTagsId, x.KaleidoscopeMosaicsId });
                    table.ForeignKey(
                        name: "FK_GroupTagKaleidoscopeMosaic_GroupTags_GroupTagsId",
                        column: x => x.GroupTagsId,
                        principalTable: "GroupTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTagKaleidoscopeMosaic_KaleidoscopeMosaics_KaleidoscopeMosaicsId",
                        column: x => x.KaleidoscopeMosaicsId,
                        principalTable: "KaleidoscopeMosaics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_GeoTagGroupTag_GroupTagsId",
                table: "GeoTagGroupTag",
                column: "GroupTagsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagKaleidoscopeMosaic_KaleidoscopeMosaicsId",
                table: "GroupTagKaleidoscopeMosaic",
                column: "KaleidoscopeMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagMolarMosaic_MolarMosaicsId",
                table: "GroupTagMolarMosaic",
                column: "MolarMosaicsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagMolecularMosaic_MolecularMosaicsId",
                table: "GroupTagMolecularMosaic",
                column: "MolecularMosaicsId");
        }
    }
}

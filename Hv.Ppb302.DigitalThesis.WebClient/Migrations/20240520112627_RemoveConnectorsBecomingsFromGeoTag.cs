using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConnectorsBecomingsFromGeoTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorTagGeoTag");

            migrationBuilder.DropColumn(
                name: "Becomings",
                table: "GeoTags");

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectorTagId",
                table: "GeoTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoTags_ConnectorTagId",
                table: "GeoTags",
                column: "ConnectorTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoTags_ConnectorTags_ConnectorTagId",
                table: "GeoTags",
                column: "ConnectorTagId",
                principalTable: "ConnectorTags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoTags_ConnectorTags_ConnectorTagId",
                table: "GeoTags");

            migrationBuilder.DropIndex(
                name: "IX_GeoTags_ConnectorTagId",
                table: "GeoTags");

            migrationBuilder.DropColumn(
                name: "ConnectorTagId",
                table: "GeoTags");

            migrationBuilder.AddColumn<string>(
                name: "Becomings",
                table: "GeoTags",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorTagGeoTag_GeoTagsId",
                table: "ConnectorTagGeoTag",
                column: "GeoTagsId");
        }
    }
}

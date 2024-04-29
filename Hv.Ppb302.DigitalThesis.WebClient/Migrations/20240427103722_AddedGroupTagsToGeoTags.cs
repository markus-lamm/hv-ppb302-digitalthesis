using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddedGroupTagsToGeoTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_GeoTagGroupTag_GroupTagsId",
                table: "GeoTagGroupTag",
                column: "GroupTagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTagGroupTag");
        }
    }
}

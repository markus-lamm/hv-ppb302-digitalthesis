using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class Addedkaleidoscopemoasicmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KaleidoscopeMosaics",
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
                    table.PrimaryKey("PK_KaleidoscopeMosaics", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_GroupTagKaleidoscopeMosaic_KaleidoscopeMosaicsId",
                table: "GroupTagKaleidoscopeMosaic",
                column: "KaleidoscopeMosaicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupTagKaleidoscopeMosaic");

            migrationBuilder.DropTable(
                name: "KaleidoscopeMosaics");
        }
    }
}

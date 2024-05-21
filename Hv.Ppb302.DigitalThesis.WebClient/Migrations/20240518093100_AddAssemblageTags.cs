using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddAssemblageTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssemblageTagId",
                table: "MolecularMosaics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssemblageTagId",
                table: "MolarMosaics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssemblageTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblageTags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MolecularMosaics_AssemblageTagId",
                table: "MolecularMosaics",
                column: "AssemblageTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MolarMosaics_AssemblageTagId",
                table: "MolarMosaics",
                column: "AssemblageTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_MolarMosaics_AssemblageTags_AssemblageTagId",
                table: "MolarMosaics",
                column: "AssemblageTagId",
                principalTable: "AssemblageTags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MolecularMosaics_AssemblageTags_AssemblageTagId",
                table: "MolecularMosaics",
                column: "AssemblageTagId",
                principalTable: "AssemblageTags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MolarMosaics_AssemblageTags_AssemblageTagId",
                table: "MolarMosaics");

            migrationBuilder.DropForeignKey(
                name: "FK_MolecularMosaics_AssemblageTags_AssemblageTagId",
                table: "MolecularMosaics");

            migrationBuilder.DropTable(
                name: "AssemblageTags");

            migrationBuilder.DropIndex(
                name: "IX_MolecularMosaics_AssemblageTagId",
                table: "MolecularMosaics");

            migrationBuilder.DropIndex(
                name: "IX_MolarMosaics_AssemblageTagId",
                table: "MolarMosaics");

            migrationBuilder.DropColumn(
                name: "AssemblageTagId",
                table: "MolecularMosaics");

            migrationBuilder.DropColumn(
                name: "AssemblageTagId",
                table: "MolarMosaics");
        }
    }
}

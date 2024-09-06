using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hv.Ppb302.DigitalThesis.WebClient.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YearlyVisits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Visits = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyVisits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyVisits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearlyVisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Visits = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyVisits_YearlyVisits_YearlyVisitId",
                        column: x => x.YearlyVisitId,
                        principalTable: "YearlyVisits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyVisits_YearlyVisitId",
                table: "MonthlyVisits",
                column: "YearlyVisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyVisits");

            migrationBuilder.DropTable(
                name: "YearlyVisits");
        }
    }
}

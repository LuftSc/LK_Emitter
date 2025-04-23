using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExternalOrderReportService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_reeRep_and_divList_reports_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DividendListReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuerId = table.Column<int>(type: "integer", nullable: false),
                    DtClo = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendListReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReeRepReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuerId = table.Column<int>(type: "integer", nullable: false),
                    ProcUk = table.Column<int>(type: "integer", nullable: false),
                    NomList = table.Column<bool>(type: "boolean", nullable: false),
                    DtMod = table.Column<DateOnly>(type: "date", nullable: false),
                    OneProcMode = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReeRepReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendListReports");

            migrationBuilder.DropTable(
                name: "ReeRepReports");
        }
    }
}

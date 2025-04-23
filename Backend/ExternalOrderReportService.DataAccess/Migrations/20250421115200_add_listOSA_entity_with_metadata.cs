using System;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExternalOrderReportService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_listOSA_entity_with_metadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "OrderReports",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ListOSAReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuerId = table.Column<int>(type: "integer", nullable: false),
                    DtMod = table.Column<DateOnly>(type: "date", nullable: false),
                    NomList = table.Column<bool>(type: "boolean", nullable: false),
                    IsCategMeeting = table.Column<bool>(type: "boolean", nullable: false),
                    IsRangeMeeting = table.Column<bool>(type: "boolean", nullable: false),
                    Dt_Begsobr = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<ListOSAMetadata>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOSAReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOSAReports");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "OrderReports",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExternalOrderReportService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_orderType_to_report_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "OrderReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "OrderReports");
        }
    }
}

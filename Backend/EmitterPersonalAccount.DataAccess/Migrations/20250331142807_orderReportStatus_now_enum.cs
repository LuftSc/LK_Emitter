using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class orderReportStatus_now_enum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderReports_ReportOrderStatus_StatusId",
                table: "OrderReports");

            migrationBuilder.DropTable(
                name: "ReportOrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_OrderReports_StatusId",
                table: "OrderReports");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrderReports");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderReports");

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "OrderReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ReportOrderStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportOrderStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ReportOrderStatus",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1L, "Successfull" },
                    { 2L, "Processing" },
                    { 3L, "Failed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderReports_StatusId",
                table: "OrderReports",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReports_ReportOrderStatus_StatusId",
                table: "OrderReports",
                column: "StatusId",
                principalTable: "ReportOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

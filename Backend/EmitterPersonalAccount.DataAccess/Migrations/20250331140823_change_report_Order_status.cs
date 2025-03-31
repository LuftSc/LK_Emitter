using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_report_Order_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ReportOrderStatus",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1L, "Successfull" },
                    { 2L, "Processing" },
                    { 3L, "Failed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReportOrderStatus",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ReportOrderStatus",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ReportOrderStatus",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}

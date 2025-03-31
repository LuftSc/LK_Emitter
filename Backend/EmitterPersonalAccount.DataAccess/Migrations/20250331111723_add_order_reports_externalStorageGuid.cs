using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_order_reports_externalStorageGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalStorageId",
                table: "OrderReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalStorageId",
                table: "OrderReports");
        }
    }
}

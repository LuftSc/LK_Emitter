using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class remove_field_issuerId_from_userLogAction_entity_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Actions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Actions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

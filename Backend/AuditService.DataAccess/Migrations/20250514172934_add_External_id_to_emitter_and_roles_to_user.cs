using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_External_id_to_emitter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Emitters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Emitters");
        }
    }
}

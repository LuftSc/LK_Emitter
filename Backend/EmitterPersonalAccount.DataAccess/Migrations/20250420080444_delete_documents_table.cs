using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class delete_documents_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmitterId",
                table: "Document",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Document",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document",
                column: "EmitterId",
                principalTable: "Emitters",
                principalColumn: "EMIT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Document");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmitterId",
                table: "Document",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document",
                column: "EmitterId",
                principalTable: "Emitters",
                principalColumn: "EMIT_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

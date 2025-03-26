using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class bind_documents_to_emitter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmitterId",
                table: "Document",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmitterSended",
                table: "Document",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Document_EmitterId",
                table: "Document",
                column: "EmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document",
                column: "EmitterId",
                principalTable: "Emitters",
                principalColumn: "EMIT_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Emitters_EmitterId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_EmitterId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "EmitterId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "IsEmitterSended",
                table: "Document");
        }
    }
}

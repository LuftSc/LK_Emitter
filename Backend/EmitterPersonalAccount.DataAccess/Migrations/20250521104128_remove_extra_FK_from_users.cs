using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class remove_extra_FK_from_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Users_UserId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RegistratorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Document_UserId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "RegistratorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmitterSended",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Document",
                newName: "SenderId");

            migrationBuilder.AddColumn<int>(
                name: "SenderRole",
                table: "Document",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderRole",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Document",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "RegistratorId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmitterSended",
                table: "Document",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegistratorId",
                table: "Users",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId",
                table: "Document",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Users_UserId",
                table: "Document",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users",
                column: "RegistratorId",
                principalTable: "Registrator",
                principalColumn: "REGISTRATOR_ID");
        }
    }
}

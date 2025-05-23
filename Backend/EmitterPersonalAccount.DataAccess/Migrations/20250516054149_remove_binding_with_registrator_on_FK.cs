using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class remove_binding_with_registrator_on_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emitters_Registrator_RegistratorId",
                table: "Emitters");

            migrationBuilder.DropIndex(
                name: "IX_Emitters_RegistratorId",
                table: "Emitters");

            migrationBuilder.DropColumn(
                name: "RegistratorId",
                table: "Emitters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistratorId",
                table: "Emitters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Emitters_RegistratorId",
                table: "Emitters",
                column: "RegistratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emitters_Registrator_RegistratorId",
                table: "Emitters",
                column: "RegistratorId",
                principalTable: "Registrator",
                principalColumn: "REGISTRATOR_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_nullable_user_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegistratorId",
                table: "Users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users",
                column: "RegistratorId",
                principalTable: "Registrator",
                principalColumn: "REGISTRATOR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegistratorId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users",
                column: "RegistratorId",
                principalTable: "Registrator",
                principalColumn: "REGISTRATOR_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

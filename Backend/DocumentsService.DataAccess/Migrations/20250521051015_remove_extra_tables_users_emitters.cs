using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentsService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class remove_extra_tables_users_emitters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Registrator");

            migrationBuilder.DropIndex(
                name: "IX_Documents_UserId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsEmitterSended",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Documents",
                newName: "SenderId");

            migrationBuilder.AddColumn<int>(
                name: "SenderRole",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderRole",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Documents",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmitterSended",
                table: "Documents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Registrator",
                columns: table => new
                {
                    REGISTRATOR_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_COUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IND = table.Column<int>(type: "integer", nullable: false),
                    COD_REGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    DT_OGRN = table.Column<DateOnly>(type: "date", nullable: false),
                    GIV_OGRN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OGRN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrator", x => x.REGISTRATOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Registrator_RegistratorId",
                        column: x => x.RegistratorId,
                        principalTable: "Registrator",
                        principalColumn: "REGISTRATOR_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegistratorId",
                table: "Users",
                column: "RegistratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

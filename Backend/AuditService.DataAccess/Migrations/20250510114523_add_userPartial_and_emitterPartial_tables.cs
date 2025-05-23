using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_userPartial_and_emitterPartial_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emitters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IO_PERS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    INN = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    COD_CITIZEN = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    F_PERS = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    DT_OGRN = table.Column<DateOnly>(type: "date", nullable: false),
                    GIV_OGRN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OGRN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GIVEN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    S_N = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DT_DOC = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(1, 1, 1))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEmitter",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmitterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmitter", x => new { x.UserId, x.EmitterId });
                    table.ForeignKey(
                        name: "FK_UserEmitter_Emitters_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Emitters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEmitter_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEmitter_EmitterId",
                table: "UserEmitter",
                column: "EmitterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEmitter");

            migrationBuilder.DropTable(
                name: "Emitters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

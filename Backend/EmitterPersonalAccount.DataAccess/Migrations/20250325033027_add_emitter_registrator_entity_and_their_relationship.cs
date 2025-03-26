using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_emitter_registrator_entity_and_their_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistratorId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Registrator",
                columns: table => new
                {
                    REGISTRATOR_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_PCOUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    PIND = table.Column<int>(type: "integer", nullable: false),
                    COD_PREGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    DT_OGRN = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GIV_OGRN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OGRN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrator", x => x.REGISTRATOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "Emitters",
                columns: table => new
                {
                    EMIT_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ONLY_PERS = table.Column<bool>(type: "boolean", nullable: false),
                    UK = table.Column<long>(type: "bigint", nullable: false),
                    PUBLIC_INFO = table.Column<bool>(type: "boolean", nullable: false),
                    POST_INF = table.Column<string>(type: "text", nullable: false),
                    MEET_NOTIFY = table.Column<string>(type: "text", nullable: false),
                    TYP_KLS = table.Column<string>(type: "text", nullable: false),
                    INFO = table.Column<string>(type: "text", nullable: false),
                    RegistratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    BIC = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BINN = table.Column<string>(type: "text", nullable: false),
                    BNAME = table.Column<string>(type: "text", nullable: false),
                    BCOR_SCH = table.Column<string>(type: "text", nullable: false),
                    FOREIGN_BCOUNTRY = table.Column<string>(type: "text", nullable: false),
                    R_S = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    BDEPART = table.Column<string>(type: "text", nullable: false),
                    BR_SCH = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TYP_PERS = table.Column<string>(type: "text", nullable: false),
                    IO_PERS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    INN = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    COD_CITIZEN = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    F_PERS = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    DT_OGRN = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GIV_OGRN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OGRN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GIVEN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    S_N = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DT_DOC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_COUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IND = table.Column<int>(type: "integer", nullable: false),
                    COD_REGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: false),
                    PADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_PCOUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    PIND = table.Column<int>(type: "integer", nullable: false),
                    COD_PREGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    RECGIVEN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    RECINN = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    RECNAME = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitters", x => x.EMIT_ID);
                    table.ForeignKey(
                        name: "FK_Emitters_Registrator_RegistratorId",
                        column: x => x.RegistratorId,
                        principalTable: "Registrator",
                        principalColumn: "REGISTRATOR_ID",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "EMIT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEmitter_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegistratorId",
                table: "Users",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Emitters_RegistratorId",
                table: "Emitters",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEmitter_EmitterId",
                table: "UserEmitter",
                column: "EmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users",
                column: "RegistratorId",
                principalTable: "Registrator",
                principalColumn: "REGISTRATOR_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Registrator_RegistratorId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserEmitter");

            migrationBuilder.DropTable(
                name: "Emitters");

            migrationBuilder.DropTable(
                name: "Registrator");

            migrationBuilder.DropIndex(
                name: "IX_Users_RegistratorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegistratorId",
                table: "Users");
        }
    }
}

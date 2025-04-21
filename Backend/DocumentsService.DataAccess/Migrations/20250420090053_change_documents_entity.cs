using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentsService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_documents_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Emitters_EmitterId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "EmitterUser");

            migrationBuilder.DropTable(
                name: "OrderReport");

            migrationBuilder.DropTable(
                name: "Emitters");

            migrationBuilder.DropIndex(
                name: "IX_Documents_EmitterId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "EmitterId",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Documents");

            migrationBuilder.AddColumn<Guid>(
                name: "EmitterId",
                table: "Documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Emitters",
                columns: table => new
                {
                    EMIT_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    INFO = table.Column<string>(type: "text", nullable: true),
                    UK = table.Column<long>(type: "bigint", nullable: false),
                    TYP_KLS = table.Column<string>(type: "text", nullable: false),
                    PUBLIC_INFO = table.Column<bool>(type: "boolean", nullable: false),
                    ONLY_PERS = table.Column<bool>(type: "boolean", nullable: false),
                    IssuerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEET_NOTIFY = table.Column<string>(type: "text", nullable: true),
                    POST_INF = table.Column<string>(type: "text", nullable: false),
                    BIC = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BINN = table.Column<string>(type: "text", nullable: true),
                    BNAME = table.Column<string>(type: "text", nullable: false),
                    BCOR_SCH = table.Column<string>(type: "text", nullable: false),
                    FOREIGN_BCOUNTRY = table.Column<string>(type: "text", nullable: true),
                    R_S = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    BDEPART = table.Column<string>(type: "text", nullable: true),
                    BR_SCH = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TYP_PERS = table.Column<string>(type: "text", nullable: false),
                    IO_PERS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    INN = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    COD_CITIZEN = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    F_PERS = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    DT_OGRN = table.Column<DateOnly>(type: "date", nullable: false),
                    GIV_OGRN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OGRN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GIVEN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    S_N = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DT_DOC = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(1, 1, 1)),
                    ADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_COUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IND = table.Column<int>(type: "integer", nullable: false),
                    COD_REGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: true),
                    PADDRESS = table.Column<string>(type: "character varying(110)", maxLength: 110, nullable: false),
                    COD_PCOUNTRY = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    PIND = table.Column<int>(type: "integer", nullable: false),
                    COD_PREGION = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    RECGIVEN = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    RECINN = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    RECNAME = table.Column<string>(type: "text", nullable: true)
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
                name: "EmitterUser",
                columns: table => new
                {
                    EmittersId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmitterUser", x => new { x.EmittersId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_EmitterUser_Emitters_EmittersId",
                        column: x => x.EmittersId,
                        principalTable: "Emitters",
                        principalColumn: "EMIT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmitterUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmitterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalStorageId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderReport_Emitters_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Emitters",
                        principalColumn: "EMIT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_EmitterId",
                table: "Documents",
                column: "EmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Emitters_RegistratorId",
                table: "Emitters",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmitterUser_UsersId",
                table: "EmitterUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderReport_EmitterId",
                table: "OrderReport",
                column: "EmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Emitters_EmitterId",
                table: "Documents",
                column: "EmitterId",
                principalTable: "Emitters",
                principalColumn: "EMIT_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

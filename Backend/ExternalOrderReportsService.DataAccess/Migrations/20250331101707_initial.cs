using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExternalOrderReportsService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OGRN_Number = table.Column<string>(type: "text", nullable: false),
                    OGRN_DateOfAssignment = table.Column<DateOnly>(type: "date", nullable: false),
                    OGRN_Issuer = table.Column<string>(type: "text", nullable: false),
                    Location_Country = table.Column<string>(type: "text", nullable: false),
                    Location_Index = table.Column<int>(type: "integer", nullable: false),
                    Location_Region = table.Column<string>(type: "text", nullable: false),
                    Location_Address_City = table.Column<string>(type: "text", nullable: false),
                    Location_Address_Street = table.Column<string>(type: "text", nullable: false),
                    Location_Address_HomeNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportOrderStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportOrderStatus", x => x.Id);
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
                    MEET_NOTIFY = table.Column<string>(type: "text", nullable: true),
                    TYP_KLS = table.Column<string>(type: "text", nullable: false),
                    INFO = table.Column<string>(type: "text", nullable: true),
                    RegistratorId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RegistratorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Registrator_RegistratorId",
                        column: x => x.RegistratorId,
                        principalTable: "Registrator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmitterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderReports_Emitters_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Emitters",
                        principalColumn: "EMIT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderReports_ReportOrderStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ReportOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEmitterSended = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    EmitterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Emitters_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Emitters",
                        principalColumn: "EMIT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
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
                        name: "FK_EmitterUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_EmitterId",
                table: "Document",
                column: "EmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId",
                table: "Document",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Emitters_RegistratorId",
                table: "Emitters",
                column: "RegistratorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmitterUser_UsersId",
                table: "EmitterUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderReports_EmitterId",
                table: "OrderReports",
                column: "EmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderReports_StatusId",
                table: "OrderReports",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RegistratorId",
                table: "User",
                column: "RegistratorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "EmitterUser");

            migrationBuilder.DropTable(
                name: "OrderReports");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Emitters");

            migrationBuilder.DropTable(
                name: "ReportOrderStatus");

            migrationBuilder.DropTable(
                name: "Registrator");
        }
    }
}

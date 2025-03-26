using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmitterPersonalAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class change_paddress_to_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PIND",
                table: "Registrator",
                newName: "IND");

            migrationBuilder.RenameColumn(
                name: "PADDRESS",
                table: "Registrator",
                newName: "ADDRESS");

            migrationBuilder.RenameColumn(
                name: "COD_PREGION",
                table: "Registrator",
                newName: "COD_REGION");

            migrationBuilder.RenameColumn(
                name: "COD_PCOUNTRY",
                table: "Registrator",
                newName: "COD_COUNTRY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IND",
                table: "Registrator",
                newName: "PIND");

            migrationBuilder.RenameColumn(
                name: "COD_REGION",
                table: "Registrator",
                newName: "COD_PREGION");

            migrationBuilder.RenameColumn(
                name: "COD_COUNTRY",
                table: "Registrator",
                newName: "COD_PCOUNTRY");

            migrationBuilder.RenameColumn(
                name: "ADDRESS",
                table: "Registrator",
                newName: "PADDRESS");
        }
    }
}

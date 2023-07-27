using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColunaGuidCobrancaIdParaGuidId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CobrancaId",
                table: "Cobrancas",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cobrancas",
                newName: "CobrancaId");
        }
    }
}

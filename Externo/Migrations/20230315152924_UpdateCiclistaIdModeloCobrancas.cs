using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCiclistaIdModeloCobrancas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CiclistaId",
                table: "Cobrancas",
                newName: "Ciclista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ciclista",
                table: "Cobrancas",
                newName: "CiclistaId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTipoCiclistaModeloCobranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciclista",
                table: "Cobrancas");

            migrationBuilder.AddColumn<Guid>(
                name: "CiclistaId",
                table: "Cobrancas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CiclistaId",
                table: "Cobrancas");

            migrationBuilder.AddColumn<int>(
                name: "Ciclista",
                table: "Cobrancas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

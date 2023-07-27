using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoNoModeloCobranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraFinalizacao",
                table: "Cobrancas");

            migrationBuilder.DropColumn(
                name: "HoraSolicitacao",
                table: "Cobrancas");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cobrancas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HoraFinalizacao",
                table: "Cobrancas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraSolicitacao",
                table: "Cobrancas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cobrancas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

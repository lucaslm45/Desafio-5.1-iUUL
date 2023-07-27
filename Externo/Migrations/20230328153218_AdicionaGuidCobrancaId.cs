using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaGuidCobrancaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cobrancas",
                table: "Cobrancas");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cobrancas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "CobrancaId",
                table: "Cobrancas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cobrancas",
                table: "Cobrancas",
                column: "CobrancaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cobrancas",
                table: "Cobrancas");

            migrationBuilder.DropColumn(
                name: "CobrancaId",
                table: "Cobrancas");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cobrancas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cobrancas",
                table: "Cobrancas",
                column: "Id");
        }
    }
}

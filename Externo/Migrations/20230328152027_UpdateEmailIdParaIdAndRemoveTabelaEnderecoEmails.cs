using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailIdParaIdAndRemoveTabelaEnderecoEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnderecoEmails");

            migrationBuilder.RenameColumn(
                name: "EmailId",
                table: "Emails",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Emails",
                newName: "EmailId");

            migrationBuilder.CreateTable(
                name: "EnderecoEmails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    HoraValidacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Verificado = table.Column<bool>(type: "boolean", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoEmails", x => x.Id);
                });
        }
    }
}

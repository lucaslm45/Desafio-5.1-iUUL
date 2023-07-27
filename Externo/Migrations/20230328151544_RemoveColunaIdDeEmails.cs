using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Externo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColunaIdDeEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Emails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Emails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katana.Migrations
{
    /// <inheritdoc />
    public partial class EnvelopeHexColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HexColor",
                table: "Envelope",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HexColor",
                table: "Envelope");
        }
    }
}

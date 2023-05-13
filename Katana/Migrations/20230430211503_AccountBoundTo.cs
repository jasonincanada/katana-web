using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katana.Migrations
{
    /// <inheritdoc />
    public partial class AccountBoundTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoundToId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_BoundToId",
                table: "Account",
                column: "BoundToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Envelope_BoundToId",
                table: "Account",
                column: "BoundToId",
                principalTable: "Envelope",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Envelope_BoundToId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_BoundToId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "BoundToId",
                table: "Account");
        }
    }
}

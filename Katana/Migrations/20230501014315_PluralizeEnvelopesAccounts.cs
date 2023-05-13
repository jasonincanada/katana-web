using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katana.Migrations
{
    /// <inheritdoc />
    public partial class PluralizeEnvelopesAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Envelope_BoundToId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Account_AccountId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Envelope",
                table: "Envelope");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Envelope",
                newName: "Envelopes");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_BoundToId",
                table: "Accounts",
                newName: "IX_Accounts_BoundToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Envelopes",
                table: "Envelopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Envelopes_BoundToId",
                table: "Accounts",
                column: "BoundToId",
                principalTable: "Envelopes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Accounts_AccountId",
                table: "Entries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Envelopes_BoundToId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Accounts_AccountId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Envelopes",
                table: "Envelopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Envelopes",
                newName: "Envelope");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_BoundToId",
                table: "Account",
                newName: "IX_Account_BoundToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Envelope",
                table: "Envelope",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Envelope_BoundToId",
                table: "Account",
                column: "BoundToId",
                principalTable: "Envelope",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Account_AccountId",
                table: "Entries",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

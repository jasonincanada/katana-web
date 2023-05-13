using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katana.Migrations
{
    /// <inheritdoc />
    public partial class EntryTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Transactions_TransactionId",
                table: "Entries");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Transactions_TransactionId",
                table: "Entries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Transactions_TransactionId",
                table: "Entries");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Entries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Transactions_TransactionId",
                table: "Entries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}

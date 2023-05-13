using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Katana.Migrations
{
    /// <inheritdoc />
    public partial class Stash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stashes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: true),
                    ToId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stashes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stashes_Envelopes_FromId",
                        column: x => x.FromId,
                        principalTable: "Envelopes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stashes_Envelopes_ToId",
                        column: x => x.ToId,
                        principalTable: "Envelopes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stashes_FromId",
                table: "Stashes",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Stashes_ToId",
                table: "Stashes",
                column: "ToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stashes");
        }
    }
}

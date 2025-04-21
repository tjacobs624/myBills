using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillManager.Migrations
{
    /// <inheritdoc />
    public partial class AddBillerIdToBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillerId",
                table: "Bills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillerId",
                table: "Bills",
                column: "BillerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Billers_BillerId",
                table: "Bills",
                column: "BillerId",
                principalTable: "Billers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Billers_BillerId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_BillerId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillerId",
                table: "Bills");
        }
    }
}

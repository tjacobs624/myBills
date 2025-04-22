using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillManager.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathToBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Bills",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Bills");
        }
    }
}

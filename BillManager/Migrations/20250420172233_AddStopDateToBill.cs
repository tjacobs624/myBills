using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillManager.Migrations
{
    /// <inheritdoc />
    public partial class AddStopDateToBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StopDate",
                table: "Bills",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StopDate",
                table: "Bills");
        }
    }
}

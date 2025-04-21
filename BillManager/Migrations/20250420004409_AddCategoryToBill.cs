using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Payments",
                newName: "DatePaid");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AddColumn<int>(
                name: "BillId1",
                table: "Payments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartial",
                table: "Payments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RecurrenceInterval",
                table: "Bills",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Bills",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BillId1",
                table: "Payments",
                column: "BillId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bills_BillId1",
                table: "Payments",
                column: "BillId1",
                principalTable: "Bills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bills_BillId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BillId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BillId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsPartial",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "DatePaid",
                table: "Payments",
                newName: "PaymentDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "RecurrenceInterval",
                table: "Bills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}

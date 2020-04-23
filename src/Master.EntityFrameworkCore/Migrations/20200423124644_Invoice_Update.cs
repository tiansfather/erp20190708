using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Invoice_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Number",
                table: "Invoice",
                type: "decimal(20,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Invoice",
                type: "decimal(20,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TaxRate",
                table: "Invoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Invoice");
        }
    }
}

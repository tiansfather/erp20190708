using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FlowSheet_Fee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "FlowSheet",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "FlowSheet");
        }
    }
}

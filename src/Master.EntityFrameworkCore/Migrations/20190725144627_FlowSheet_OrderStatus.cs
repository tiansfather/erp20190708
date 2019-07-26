using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FlowSheet_OrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "FlowSheet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "FlowSheet");
        }
    }
}

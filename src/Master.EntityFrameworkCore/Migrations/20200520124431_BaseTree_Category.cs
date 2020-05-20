using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class BaseTree_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "BaseTree",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "BaseTree");
        }
    }
}

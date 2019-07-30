using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class UnitFeeHistory_CompanyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelCompanyName",
                table: "UnitFeeHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelCompanyName",
                table: "UnitFeeHistory");
        }
    }
}

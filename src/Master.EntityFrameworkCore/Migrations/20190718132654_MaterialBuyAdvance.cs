using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialBuyAdvance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeEndNumber",
                table: "MaterialBuy",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeStartNumber",
                table: "MaterialBuy",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeatureCode",
                table: "MaterialBuy",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeEndNumber",
                table: "MaterialBuy");

            migrationBuilder.DropColumn(
                name: "CodeStartNumber",
                table: "MaterialBuy");

            migrationBuilder.DropColumn(
                name: "FeatureCode",
                table: "MaterialBuy");
        }
    }
}

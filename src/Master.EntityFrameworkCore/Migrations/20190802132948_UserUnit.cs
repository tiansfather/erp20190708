using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class UserUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UnitId",
                table: "User",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Unit_UnitId",
                table: "User",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Unit_UnitId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UnitId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "User");
        }
    }
}

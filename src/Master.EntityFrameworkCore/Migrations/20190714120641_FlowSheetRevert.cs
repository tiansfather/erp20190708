using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FlowSheetRevert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelSheetId",
                table: "FlowSheet",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevertReason",
                table: "FlowSheet",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_RelSheetId",
                table: "FlowSheet",
                column: "RelSheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_FlowSheet_RelSheetId",
                table: "FlowSheet",
                column: "RelSheetId",
                principalTable: "FlowSheet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowSheet_FlowSheet_RelSheetId",
                table: "FlowSheet");

            migrationBuilder.DropIndex(
                name: "IX_FlowSheet_RelSheetId",
                table: "FlowSheet");

            migrationBuilder.DropColumn(
                name: "RelSheetId",
                table: "FlowSheet");

            migrationBuilder.DropColumn(
                name: "RevertReason",
                table: "FlowSheet");

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);
        }
    }
}

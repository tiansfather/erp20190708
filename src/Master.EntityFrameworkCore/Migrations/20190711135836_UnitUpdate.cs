using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class UnitUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unit_BaseTree_UnitTypeId",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Unit_UnitTypeId",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                table: "Unit");

            migrationBuilder.RenameColumn(
                name: "UnitSN",
                table: "Unit",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "SupplierType",
                table: "Unit",
                newName: "ContactPerson");

            migrationBuilder.RenameColumn(
                name: "LimitDowe",
                table: "Material",
                newName: "LimitDown");

            migrationBuilder.AddColumn<decimal>(
                name: "StartFee",
                table: "Unit",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitRank",
                table: "Unit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartFee",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "UnitRank",
                table: "Unit");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Unit",
                newName: "UnitSN");

            migrationBuilder.RenameColumn(
                name: "ContactPerson",
                table: "Unit",
                newName: "SupplierType");

            migrationBuilder.RenameColumn(
                name: "LimitDown",
                table: "Material",
                newName: "LimitDowe");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Unit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Unit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Unit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeId",
                table: "Unit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_UnitTypeId",
                table: "Unit",
                column: "UnitTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_BaseTree_UnitTypeId",
                table: "Unit",
                column: "UnitTypeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

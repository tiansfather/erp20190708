using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class unitdiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UnitMaterialDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    UnitDiscount = table.Column<int>(nullable: false),
                    UnitSellMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitMaterialDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitMaterialDiscount_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitMaterialDiscount_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitMaterialDiscount_MaterialId",
                table: "UnitMaterialDiscount",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMaterialDiscount_UnitId",
                table: "UnitMaterialDiscount",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitMaterialDiscount");

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialBuyBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialBuyBack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: false),
                    BackNumber = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBuyBack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBuyBack_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialBuyBack_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialBuyBack_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuyBack_FlowSheetId",
                table: "MaterialBuyBack",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuyBack_MaterialId",
                table: "MaterialBuyBack",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuyBack_UnitId",
                table: "MaterialBuyBack",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialBuyBack");
        }
    }
}

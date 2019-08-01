using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialSell_Advance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialSellBack",
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
                    table.PrimaryKey("PK_MaterialSellBack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialSellBack_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSellBack_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSellBack_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialSellOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: false),
                    OutNumber = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSellOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialSellOut_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSellOut_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSellOut_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellBack_FlowSheetId",
                table: "MaterialSellBack",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellBack_MaterialId",
                table: "MaterialSellBack",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellBack_UnitId",
                table: "MaterialSellBack",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellOut_FlowSheetId",
                table: "MaterialSellOut",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellOut_MaterialId",
                table: "MaterialSellOut",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellOut_UnitId",
                table: "MaterialSellOut",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialSellBack");

            migrationBuilder.DropTable(
                name: "MaterialSellOut");
        }
    }
}

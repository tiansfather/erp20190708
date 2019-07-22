using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialSell",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: false),
                    SellNumber = table.Column<int>(nullable: false),
                    OutNumber = table.Column<int>(nullable: false),
                    BackNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialSell_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSell_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSell_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialSellCart",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSellCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialSellCart_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSellCart_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSell_FlowSheetId",
                table: "MaterialSell",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSell_MaterialId",
                table: "MaterialSell",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSell_UnitId",
                table: "MaterialSell",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellCart_MaterialId",
                table: "MaterialSellCart",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSellCart_UnitId",
                table: "MaterialSellCart",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialSell");

            migrationBuilder.DropTable(
                name: "MaterialSellCart");
        }
    }
}

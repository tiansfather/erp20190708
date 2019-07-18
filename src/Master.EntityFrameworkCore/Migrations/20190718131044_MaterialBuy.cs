using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialBuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialBuy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    MaterialId = table.Column<int>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: false),
                    BuyNumber = table.Column<int>(nullable: false),
                    BackNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBuy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBuy_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialBuy_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuy_FlowSheetId",
                table: "MaterialBuy",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuy_MaterialId",
                table: "MaterialBuy",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialBuy");
        }
    }
}

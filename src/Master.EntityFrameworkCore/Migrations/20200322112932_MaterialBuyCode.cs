using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MaterialBuyCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialBuyCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    MaterialBuyId = table.Column<int>(nullable: false),
                    CodeStartNumber = table.Column<decimal>(nullable: true),
                    CodeEndNumber = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBuyCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBuyCode_MaterialBuy_MaterialBuyId",
                        column: x => x.MaterialBuyId,
                        principalTable: "MaterialBuy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuyCode_MaterialBuyId",
                table: "MaterialBuyCode",
                column: "MaterialBuyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialBuyCode");
        }
    }
}

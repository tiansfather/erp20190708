using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class StoreMaterial : Migration
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
                name: "StoreMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    Number = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_CreatorUserId",
                table: "StoreMaterial",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_DeleterUserId",
                table: "StoreMaterial",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_LastModifierUserId",
                table: "StoreMaterial",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_MaterialId",
                table: "StoreMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_StoreId",
                table: "StoreMaterial",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterial_TenantId",
                table: "StoreMaterial",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMaterial");

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

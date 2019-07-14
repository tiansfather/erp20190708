using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class StoreMaterialHistory : Migration
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
                name: "StoreMaterialHistory",
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
                    MaterialName = table.Column<string>(nullable: true),
                    Specification = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    StoreName = table.Column<string>(nullable: true),
                    MeasureMentUnitName = table.Column<string>(nullable: true),
                    ChangeType = table.Column<string>(nullable: true),
                    Number = table.Column<decimal>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMaterialHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_CreatorUserId",
                table: "StoreMaterialHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_DeleterUserId",
                table: "StoreMaterialHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_FlowSheetId",
                table: "StoreMaterialHistory",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_LastModifierUserId",
                table: "StoreMaterialHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_MaterialId",
                table: "StoreMaterialHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_StoreId",
                table: "StoreMaterialHistory",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMaterialHistory_TenantId",
                table: "StoreMaterialHistory",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMaterialHistory");

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "SystemLog",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

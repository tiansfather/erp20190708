using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class UnitFeeHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitFeeHistory",
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
                    UnitId = table.Column<int>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    RemainFee = table.Column<decimal>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    FlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitFeeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_CreatorUserId",
                table: "UnitFeeHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_DeleterUserId",
                table: "UnitFeeHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_FlowSheetId",
                table: "UnitFeeHistory",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_LastModifierUserId",
                table: "UnitFeeHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_TenantId",
                table: "UnitFeeHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFeeHistory_UnitId",
                table: "UnitFeeHistory",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitFeeHistory");
        }
    }
}

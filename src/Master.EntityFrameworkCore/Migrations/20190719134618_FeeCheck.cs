using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FeeCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeCheck",
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
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CheckNumber = table.Column<string>(nullable: true),
                    CheckFee = table.Column<decimal>(nullable: false),
                    CheckDate = table.Column<DateTime>(nullable: false),
                    CheckDaySpan = table.Column<int>(nullable: false),
                    CheckCompany = table.Column<string>(nullable: true),
                    CheckBank = table.Column<string>(nullable: true),
                    CheckStatus = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    InFlowSheetId = table.Column<int>(nullable: true),
                    OutFlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeCheck_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCheck_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCheck_FlowSheet_InFlowSheetId",
                        column: x => x.InFlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCheck_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCheck_FlowSheet_OutFlowSheetId",
                        column: x => x.OutFlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCheck_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_CreatorUserId",
                table: "FeeCheck",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_DeleterUserId",
                table: "FeeCheck",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_InFlowSheetId",
                table: "FeeCheck",
                column: "InFlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_LastModifierUserId",
                table: "FeeCheck",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_OutFlowSheetId",
                table: "FeeCheck",
                column: "OutFlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCheck_TenantId",
                table: "FeeCheck",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeCheck");
        }
    }
}

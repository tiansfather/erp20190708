using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FeeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "UnitFeeHistory",
                newName: "ChangeType");

            migrationBuilder.CreateTable(
                name: "FeeAccountHistory",
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
                    FeeAccountId = table.Column<int>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    RemainFee = table.Column<decimal>(nullable: false),
                    ChangeType = table.Column<string>(nullable: true),
                    FlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeAccountHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_FeeAccount_FeeAccountId",
                        column: x => x.FeeAccountId,
                        principalTable: "FeeAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_CreatorUserId",
                table: "FeeAccountHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_DeleterUserId",
                table: "FeeAccountHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_FeeAccountId",
                table: "FeeAccountHistory",
                column: "FeeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_FlowSheetId",
                table: "FeeAccountHistory",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_LastModifierUserId",
                table: "FeeAccountHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_TenantId",
                table: "FeeAccountHistory",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeAccountHistory");

            migrationBuilder.RenameColumn(
                name: "ChangeType",
                table: "UnitFeeHistory",
                newName: "Content");
        }
    }
}

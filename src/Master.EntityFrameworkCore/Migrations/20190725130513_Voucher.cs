using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    VoucherStatus = table.Column<int>(nullable: false),
                    RelSheetSN = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voucher_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voucher_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voucher_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voucher_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voucher_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_CreatorUserId",
                table: "Voucher",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_DeleterUserId",
                table: "Voucher",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_LastModifierUserId",
                table: "Voucher",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_TenantId",
                table: "Voucher",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_UnitId",
                table: "Voucher",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voucher");
        }
    }
}

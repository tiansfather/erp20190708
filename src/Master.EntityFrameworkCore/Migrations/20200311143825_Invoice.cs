using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
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
                    InvoiceStatus = table.Column<int>(nullable: false),
                    SheetSN = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    SellUnitName = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    BuyUnitName = table.Column<string>(nullable: true),
                    BuyUnitTaxNumber = table.Column<string>(nullable: true),
                    BuyUnitAddress = table.Column<string>(nullable: true),
                    BuyUnitPhone = table.Column<string>(nullable: true),
                    BuyUnitBank = table.Column<string>(nullable: true),
                    BuyUnitBankAccount = table.Column<string>(nullable: true),
                    OrderType = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoice_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CreatorUserId",
                table: "Invoice",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_DeleterUserId",
                table: "Invoice",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_LastModifierUserId",
                table: "Invoice",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TenantId",
                table: "Invoice",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UnitId",
                table: "Invoice",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoice");
        }
    }
}

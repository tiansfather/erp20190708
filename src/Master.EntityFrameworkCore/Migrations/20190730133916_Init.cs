using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationLanguage",
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
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLanguageText",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageName = table.Column<string>(maxLength: 10, nullable: false),
                    Source = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(maxLength: 67108864, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLanguageText", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    Parameters = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    BrowserInfo = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Edition",
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
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAttempt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrPhoneNumber = table.Column<string>(nullable: true),
                    ClientIpAddress = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    BrowserInfo = table.Column<string>(nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginAttempt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureSetting",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureSetting_Edition_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Edition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Material",
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
                    Name = table.Column<string>(nullable: true),
                    Specification = table.Column<string>(nullable: true),
                    MaterialTypeId = table.Column<int>(nullable: true),
                    MaterialNature = table.Column<int>(nullable: false),
                    MaterialDIYType = table.Column<int>(nullable: false),
                    MeasureMentUnit = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    LimitDown = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DefaultBuyDiscount = table.Column<decimal>(nullable: true),
                    DefaultSellDiscount = table.Column<decimal>(nullable: true),
                    SellDiscount1 = table.Column<decimal>(nullable: true),
                    SellDiscount2 = table.Column<decimal>(nullable: true),
                    SellDiscount3 = table.Column<decimal>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    TotalNumber = table.Column<decimal>(nullable: false),
                    IsDiyed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
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
                    EditionId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Property = table.Column<string>(type: "json", nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenant_Edition_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Edition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemLog",
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
                    LogName = table.Column<string>(nullable: true),
                    LogGroup = table.Column<string>(nullable: true),
                    LogEntityIdentifier = table.Column<string>(nullable: true),
                    LogContent = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemLog_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeAccountHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    UnitId = table.Column<int>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    FeeDirection = table.Column<int>(nullable: false),
                    RemainFee = table.Column<decimal>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<string>(nullable: true),
                    FlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeAccountHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeAccountHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstance",
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
                    FlowSchemeId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    InstanceName = table.Column<string>(nullable: true),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityType = table.Column<int>(nullable: true),
                    ActivityName = table.Column<string>(nullable: true),
                    PreviousId = table.Column<string>(nullable: true),
                    SchemeContent = table.Column<string>(nullable: true),
                    FormContent = table.Column<string>(nullable: true),
                    FormData = table.Column<string>(nullable: true),
                    FlowFormId = table.Column<int>(nullable: false),
                    FormType = table.Column<int>(nullable: false),
                    FlowLevel = table.Column<int>(nullable: false),
                    InstanceStatus = table.Column<int>(nullable: false),
                    MakerList = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowInstance_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowScheme",
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
                    SchemeCode = table.Column<string>(nullable: true),
                    SchemeName = table.Column<string>(nullable: true),
                    SchemeType = table.Column<string>(nullable: true),
                    SchemeVersion = table.Column<string>(nullable: true),
                    SchemeCanUser = table.Column<string>(nullable: true),
                    SchemeContent = table.Column<string>(nullable: true),
                    FlowFormId = table.Column<int>(nullable: false),
                    AuthorizeType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowScheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowScheme_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstanceOperationHistory",
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
                    FlowInstanceId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstanceOperationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowInstanceOperationHistory_FlowInstance_FlowInstanceId",
                        column: x => x.FlowInstanceId,
                        principalTable: "FlowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowInstanceOperationHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstanceTransitionHistory",
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
                    FlowInstanceId = table.Column<int>(nullable: false),
                    FromNodeId = table.Column<string>(nullable: true),
                    FromNodeType = table.Column<int>(nullable: true),
                    FromNodeName = table.Column<string>(nullable: true),
                    ToNodeId = table.Column<string>(nullable: true),
                    ToNodeType = table.Column<int>(nullable: true),
                    ToNodeName = table.Column<string>(nullable: true),
                    TransitionSate = table.Column<int>(nullable: false),
                    InstanceStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstanceTransitionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowInstanceTransitionHistory_FlowInstance_FlowInstanceId",
                        column: x => x.FlowInstanceId,
                        principalTable: "FlowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowInstanceTransitionHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowSheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FlowInstanceId = table.Column<int>(nullable: true),
                    FormKey = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    BusinessSN = table.Column<string>(nullable: true),
                    SheetName = table.Column<string>(nullable: true),
                    SheetSN = table.Column<string>(nullable: true),
                    SheetDate = table.Column<DateTime>(nullable: false),
                    BusinessType = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    HandlerId = table.Column<long>(nullable: true),
                    SheetNature = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    SheetStatus = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    RelSheetId = table.Column<int>(nullable: true),
                    RevertReason = table.Column<string>(nullable: true),
                    OrderStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowSheet_FlowInstance_FlowInstanceId",
                        column: x => x.FlowInstanceId,
                        principalTable: "FlowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowSheet_FlowSheet_RelSheetId",
                        column: x => x.RelSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowSheet_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_FeeCheck_FlowSheet_InFlowSheetId",
                        column: x => x.InFlowSheetId,
                        principalTable: "FlowSheet",
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

            migrationBuilder.CreateTable(
                name: "FlowSheetContent",
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
                    SheetId = table.Column<int>(nullable: false),
                    SheetContentId = table.Column<int>(nullable: true),
                    RelativeSheetContentId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowSheetContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowSheetContent_FlowSheetContent_RelativeSheetContentId",
                        column: x => x.RelativeSheetContentId,
                        principalTable: "FlowSheetContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowSheetContent_FlowSheet_SheetId",
                        column: x => x.SheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowSheetContent_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialBuy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    FlowSheetId = table.Column<int>(nullable: false),
                    BuyNumber = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    BackNumber = table.Column<int>(nullable: false),
                    FeatureCode = table.Column<string>(nullable: true),
                    CodeStartNumber = table.Column<string>(nullable: true),
                    CodeEndNumber = table.Column<string>(nullable: true)
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
                });

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
                        name: "FK_StoreMaterialHistory_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterialHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitFeeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    RemainFee = table.Column<decimal>(nullable: false),
                    ChangeType = table.Column<string>(nullable: true),
                    FlowSheetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitFeeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_FlowSheet_FlowSheetId",
                        column: x => x.FlowSheetId,
                        principalTable: "FlowSheet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitFeeHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
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
                });

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
                        name: "FK_StoreMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMaterial_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitMaterialDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    UnitDiscount = table.Column<int>(nullable: false),
                    UnitSellMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitMaterialDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitMaterialDiscount_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColumnInfo",
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
                    TenantId = table.Column<int>(nullable: false),
                    ModuleInfoId = table.Column<int>(nullable: false),
                    ColumnType = table.Column<int>(nullable: false),
                    EnableFieldPermission = table.Column<bool>(nullable: false),
                    ControlFormat = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    ColumnKey = table.Column<string>(nullable: true),
                    ColumnName = table.Column<string>(nullable: true),
                    Templet = table.Column<string>(nullable: true),
                    MaxFileNumber = table.Column<int>(nullable: false),
                    IsInterColumn = table.Column<bool>(nullable: false),
                    IsSystemColumn = table.Column<bool>(nullable: false),
                    DisplayFormat = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    VerifyRules = table.Column<string>(nullable: true),
                    Renderer = table.Column<string>(nullable: true),
                    ValuePath = table.Column<string>(nullable: true),
                    DisplayPath = table.Column<string>(nullable: true),
                    DictionaryName = table.Column<string>(nullable: true),
                    CustomizeControl = table.Column<string>(nullable: true),
                    ControlParameter = table.Column<string>(nullable: true),
                    IsShownInList = table.Column<bool>(nullable: false),
                    IsShownInAdd = table.Column<bool>(nullable: false),
                    IsShownInEdit = table.Column<bool>(nullable: false),
                    IsShownInMultiEdit = table.Column<bool>(nullable: false),
                    IsShownInAdvanceSearch = table.Column<bool>(nullable: false),
                    IsShownInView = table.Column<bool>(nullable: false),
                    IsEnableSort = table.Column<bool>(nullable: false),
                    RelativeDataType = table.Column<int>(nullable: false),
                    RelativeDataString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleButton",
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
                    ModuleInfoId = table.Column<int>(nullable: false),
                    ButtonKey = table.Column<string>(nullable: true),
                    ButtonName = table.Column<string>(nullable: true),
                    ButtonClass = table.Column<string>(nullable: true),
                    TitleTemplet = table.Column<string>(nullable: true),
                    ButtonActionType = table.Column<int>(nullable: false),
                    ButtonType = table.Column<int>(nullable: false),
                    ButtonActionUrl = table.Column<string>(nullable: true),
                    ButtonActionParam = table.Column<string>(nullable: true),
                    ConfirmMsg = table.Column<string>(nullable: true),
                    ButtonScript = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    RequirePermission = table.Column<bool>(nullable: false),
                    ClientShowCondition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleButton", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleButton_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleData",
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
                    ModuleInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
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
                    Status = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    ProjectSN = table.Column<string>(nullable: true),
                    CustomerProjectSN = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    ProjectType = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    RequireDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(maxLength: 2, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    WorkNumber = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Degree = table.Column<string>(nullable: true),
                    Nation = table.Column<string>(nullable: true),
                    JobDateStart = table.Column<DateTime>(nullable: true),
                    JobDateEnd = table.Column<DateTime>(nullable: true),
                    Marriage = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseTree",
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
                    TenantId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    BriefCode = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Nature = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_BaseTree_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseType",
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
                    TenantId = table.Column<int>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    BriefName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseType_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DictionaryName = table.Column<string>(nullable: true),
                    DictionaryContent = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeAccount",
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
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StartFee = table.Column<decimal>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeAccount_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccount_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccount_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeAccount_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
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
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<decimal>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowForm",
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
                    FormKey = table.Column<string>(nullable: true),
                    FormName = table.Column<string>(nullable: true),
                    FormType = table.Column<int>(nullable: false),
                    FormContent = table.Column<string>(nullable: true),
                    FormHandler = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsSystemForm = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowForm_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowForm_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowForm_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowForm_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleInfo",
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
                    TenantId = table.Column<int>(nullable: false),
                    IsInterModule = table.Column<bool>(nullable: false),
                    RequiredFeature = table.Column<string>(nullable: true),
                    ModuleKey = table.Column<string>(nullable: true),
                    EntityFullName = table.Column<string>(nullable: true),
                    ModuleName = table.Column<string>(nullable: true),
                    DefaultLimit = table.Column<int>(nullable: false),
                    Limits = table.Column<string>(nullable: true),
                    SortField = table.Column<string>(nullable: true),
                    SortType = table.Column<int>(nullable: false),
                    FormScript = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notice",
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
                    TenantId = table.Column<int>(nullable: true),
                    NoticeTitle = table.Column<string>(nullable: true),
                    NoticeContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    NoticeType = table.Column<string>(nullable: true),
                    ToTenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notice_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
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
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    BriefCode = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
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
                    TenantId = table.Column<int>(nullable: true),
                    ResourceName = table.Column<string>(nullable: true),
                    ResourceType = table.Column<string>(nullable: true),
                    ResourceContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
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
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Store",
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
                    StoreCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Store_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Store_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Store_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Template",
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
                    TenantId = table.Column<int>(nullable: true),
                    TemplateName = table.Column<string>(nullable: true),
                    TemplateType = table.Column<string>(nullable: true),
                    TemplateContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
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
                    UnitName = table.Column<string>(nullable: true),
                    BriefName = table.Column<string>(nullable: true),
                    UnitNature = table.Column<int>(nullable: false),
                    StartFee = table.Column<decimal>(nullable: false),
                    Fee = table.Column<decimal>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    UnitRank = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unit_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Unit_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Unit_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Unit_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
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
                    OrderSN = table.Column<string>(nullable: true),
                    OrderName = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_AuditLog_TenantId_ExecutionDuration",
                table: "AuditLog",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_ExecutionTime",
                table: "AuditLog",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_UserId",
                table: "AuditLog",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_CreatorUserId",
                table: "BaseTree",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_DeleterUserId",
                table: "BaseTree",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_LastModifierUserId",
                table: "BaseTree",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_ParentId",
                table: "BaseTree",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_TenantId",
                table: "BaseTree",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_CreatorUserId",
                table: "BaseType",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_DeleterUserId",
                table: "BaseType",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_LastModifierUserId",
                table: "BaseType",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_TenantId",
                table: "BaseType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnInfo_ModuleInfoId",
                table: "ColumnInfo",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_CreatorUserId",
                table: "Dictionary",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_DeleterUserId",
                table: "Dictionary",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_LastModifierUserId",
                table: "Dictionary",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_TenantId",
                table: "Dictionary",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureSetting_EditionId",
                table: "FeatureSetting",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccount_CreatorUserId",
                table: "FeeAccount",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccount_DeleterUserId",
                table: "FeeAccount",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccount_LastModifierUserId",
                table: "FeeAccount",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccount_TenantId",
                table: "FeeAccount",
                column: "TenantId");

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

            migrationBuilder.CreateIndex(
                name: "IX_FeeAccountHistory_UnitId",
                table: "FeeAccountHistory",
                column: "UnitId");

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

            migrationBuilder.CreateIndex(
                name: "IX_File_CreatorUserId",
                table: "File",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_DeleterUserId",
                table: "File",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_LastModifierUserId",
                table: "File",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowForm_CreatorUserId",
                table: "FlowForm",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowForm_DeleterUserId",
                table: "FlowForm",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowForm_LastModifierUserId",
                table: "FlowForm",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowForm_TenantId",
                table: "FlowForm",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_CreatorUserId",
                table: "FlowInstance",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_DeleterUserId",
                table: "FlowInstance",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_FlowFormId",
                table: "FlowInstance",
                column: "FlowFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_FlowSchemeId",
                table: "FlowInstance",
                column: "FlowSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_LastModifierUserId",
                table: "FlowInstance",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstance_TenantId",
                table: "FlowInstance",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceOperationHistory_CreatorUserId",
                table: "FlowInstanceOperationHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceOperationHistory_DeleterUserId",
                table: "FlowInstanceOperationHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceOperationHistory_FlowInstanceId",
                table: "FlowInstanceOperationHistory",
                column: "FlowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceOperationHistory_LastModifierUserId",
                table: "FlowInstanceOperationHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceOperationHistory_TenantId",
                table: "FlowInstanceOperationHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceTransitionHistory_CreatorUserId",
                table: "FlowInstanceTransitionHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceTransitionHistory_DeleterUserId",
                table: "FlowInstanceTransitionHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceTransitionHistory_FlowInstanceId",
                table: "FlowInstanceTransitionHistory",
                column: "FlowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceTransitionHistory_LastModifierUserId",
                table: "FlowInstanceTransitionHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstanceTransitionHistory_TenantId",
                table: "FlowInstanceTransitionHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowScheme_CreatorUserId",
                table: "FlowScheme",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowScheme_DeleterUserId",
                table: "FlowScheme",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowScheme_FlowFormId",
                table: "FlowScheme",
                column: "FlowFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowScheme_LastModifierUserId",
                table: "FlowScheme",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowScheme_TenantId",
                table: "FlowScheme",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_CreatorUserId",
                table: "FlowSheet",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_DeleterUserId",
                table: "FlowSheet",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_FlowInstanceId",
                table: "FlowSheet",
                column: "FlowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_HandlerId",
                table: "FlowSheet",
                column: "HandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_LastModifierUserId",
                table: "FlowSheet",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_RelSheetId",
                table: "FlowSheet",
                column: "RelSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_TenantId",
                table: "FlowSheet",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheet_UnitId",
                table: "FlowSheet",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_CreatorUserId",
                table: "FlowSheetContent",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_DeleterUserId",
                table: "FlowSheetContent",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_LastModifierUserId",
                table: "FlowSheetContent",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_RelativeSheetContentId",
                table: "FlowSheetContent",
                column: "RelativeSheetContentId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_SheetId",
                table: "FlowSheetContent",
                column: "SheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowSheetContent_TenantId",
                table: "FlowSheetContent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_CreatorUserId",
                table: "Material",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_DeleterUserId",
                table: "Material",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_LastModifierUserId",
                table: "Material",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_MaterialTypeId",
                table: "Material",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_TenantId",
                table: "Material",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuy_FlowSheetId",
                table: "MaterialBuy",
                column: "FlowSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuy_MaterialId",
                table: "MaterialBuy",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBuy_UnitId",
                table: "MaterialBuy",
                column: "UnitId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_CreatorUserId",
                table: "ModuleButton",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_DeleterUserId",
                table: "ModuleButton",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_LastModifierUserId",
                table: "ModuleButton",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_ModuleInfoId",
                table: "ModuleButton",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_TenantId",
                table: "ModuleButton",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_CreatorUserId",
                table: "ModuleData",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_DeleterUserId",
                table: "ModuleData",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_LastModifierUserId",
                table: "ModuleData",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_ModuleInfoId",
                table: "ModuleData",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_CreatorUserId",
                table: "ModuleInfo",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_DeleterUserId",
                table: "ModuleInfo",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_LastModifierUserId",
                table: "ModuleInfo",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_CreatorUserId",
                table: "Notice",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_DeleterUserId",
                table: "Notice",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_LastModifierUserId",
                table: "Notice",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_TenantId",
                table: "Notice",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatorUserId",
                table: "Order",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeleterUserId",
                table: "Order",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_LastModifierUserId",
                table: "Order",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TenantId",
                table: "Order",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UnitId",
                table: "Order",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_CreatorUserId",
                table: "Organization",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_DeleterUserId",
                table: "Organization",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_LastModifierUserId",
                table: "Organization",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ParentId",
                table: "Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_TenantId",
                table: "Organization",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_TenantId_Name",
                table: "Permissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatorUserId",
                table: "Project",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DeleterUserId",
                table: "Project",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_LastModifierUserId",
                table: "Project",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OrderId",
                table: "Project",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_TenantId",
                table: "Project",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UnitId",
                table: "Project",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_CreatorUserId",
                table: "Resource",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_DeleterUserId",
                table: "Resource",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_LastModifierUserId",
                table: "Resource",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_TenantId",
                table: "Resource",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatorUserId",
                table: "Role",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_DeleterUserId",
                table: "Role",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastModifierUserId",
                table: "Role",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TenantId",
                table: "Role",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_TenantId_Name",
                table: "Settings",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Store_CreatorUserId",
                table: "Store",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_DeleterUserId",
                table: "Store",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_LastModifierUserId",
                table: "Store",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_TenantId",
                table: "Store",
                column: "TenantId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SystemLog_TenantId",
                table: "SystemLog",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_CreatorUserId",
                table: "Template",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_DeleterUserId",
                table: "Template",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_LastModifierUserId",
                table: "Template",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_TenantId",
                table: "Template",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_CreatorUserId",
                table: "Tenant",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_DeleterUserId",
                table: "Tenant",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_EditionId",
                table: "Tenant",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_LastModifierUserId",
                table: "Tenant",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_CreatorUserId",
                table: "Unit",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_DeleterUserId",
                table: "Unit",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_LastModifierUserId",
                table: "Unit",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_TenantId",
                table: "Unit",
                column: "TenantId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UnitMaterialDiscount_MaterialId",
                table: "UnitMaterialDiscount",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitMaterialDiscount_UnitId",
                table: "UnitMaterialDiscount",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorUserId",
                table: "User",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeleterUserId",
                table: "User",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastModifierUserId",
                table: "User",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganizationId",
                table: "User",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId",
                table: "User",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempt_UserId_TenantId",
                table: "UserLoginAttempt",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempt_TenancyName_UserNameOrPhoneNumber_Result",
                table: "UserLoginAttempt",
                columns: new[] { "TenancyName", "UserNameOrPhoneNumber", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_TenantId_RoleId",
                table: "UserRole",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_TenantId_UserId",
                table: "UserRole",
                columns: new[] { "TenantId", "UserId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Material_User_CreatorUserId",
                table: "Material",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_User_DeleterUserId",
                table: "Material",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_User_LastModifierUserId",
                table: "Material",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Tenant_TenantId",
                table: "Material",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_BaseTree_MaterialTypeId",
                table: "Material",
                column: "MaterialTypeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_CreatorUserId",
                table: "Tenant",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_DeleterUserId",
                table: "Tenant",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_LastModifierUserId",
                table: "Tenant",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_User_CreatorUserId",
                table: "FeeAccountHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_User_DeleterUserId",
                table: "FeeAccountHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_User_LastModifierUserId",
                table: "FeeAccountHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_FeeAccount_FeeAccountId",
                table: "FeeAccountHistory",
                column: "FeeAccountId",
                principalTable: "FeeAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_FlowSheet_FlowSheetId",
                table: "FeeAccountHistory",
                column: "FlowSheetId",
                principalTable: "FlowSheet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeAccountHistory_Unit_UnitId",
                table: "FeeAccountHistory",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstance_User_CreatorUserId",
                table: "FlowInstance",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstance_User_DeleterUserId",
                table: "FlowInstance",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstance_User_LastModifierUserId",
                table: "FlowInstance",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstance_FlowForm_FlowFormId",
                table: "FlowInstance",
                column: "FlowFormId",
                principalTable: "FlowForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstance_FlowScheme_FlowSchemeId",
                table: "FlowInstance",
                column: "FlowSchemeId",
                principalTable: "FlowScheme",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowScheme_User_CreatorUserId",
                table: "FlowScheme",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowScheme_User_DeleterUserId",
                table: "FlowScheme",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowScheme_User_LastModifierUserId",
                table: "FlowScheme",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowScheme_FlowForm_FlowFormId",
                table: "FlowScheme",
                column: "FlowFormId",
                principalTable: "FlowForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceOperationHistory_User_CreatorUserId",
                table: "FlowInstanceOperationHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceOperationHistory_User_DeleterUserId",
                table: "FlowInstanceOperationHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceOperationHistory_User_LastModifierUserId",
                table: "FlowInstanceOperationHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceTransitionHistory_User_CreatorUserId",
                table: "FlowInstanceTransitionHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceTransitionHistory_User_DeleterUserId",
                table: "FlowInstanceTransitionHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowInstanceTransitionHistory_User_LastModifierUserId",
                table: "FlowInstanceTransitionHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_User_CreatorUserId",
                table: "FlowSheet",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_User_DeleterUserId",
                table: "FlowSheet",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_User_HandlerId",
                table: "FlowSheet",
                column: "HandlerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_User_LastModifierUserId",
                table: "FlowSheet",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheet_Unit_UnitId",
                table: "FlowSheet",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCheck_User_CreatorUserId",
                table: "FeeCheck",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCheck_User_DeleterUserId",
                table: "FeeCheck",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCheck_User_LastModifierUserId",
                table: "FeeCheck",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheetContent_User_CreatorUserId",
                table: "FlowSheetContent",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheetContent_User_DeleterUserId",
                table: "FlowSheetContent",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowSheetContent_User_LastModifierUserId",
                table: "FlowSheetContent",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialBuy_Unit_UnitId",
                table: "MaterialBuy",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialSell_Unit_UnitId",
                table: "MaterialSell",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterialHistory_User_CreatorUserId",
                table: "StoreMaterialHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterialHistory_User_DeleterUserId",
                table: "StoreMaterialHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterialHistory_User_LastModifierUserId",
                table: "StoreMaterialHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterialHistory_Store_StoreId",
                table: "StoreMaterialHistory",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitFeeHistory_User_CreatorUserId",
                table: "UnitFeeHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitFeeHistory_User_DeleterUserId",
                table: "UnitFeeHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitFeeHistory_User_LastModifierUserId",
                table: "UnitFeeHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitFeeHistory_Unit_UnitId",
                table: "UnitFeeHistory",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialSellCart_Unit_UnitId",
                table: "MaterialSellCart",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterial_User_CreatorUserId",
                table: "StoreMaterial",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterial_User_DeleterUserId",
                table: "StoreMaterial",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterial_User_LastModifierUserId",
                table: "StoreMaterial",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMaterial_Store_StoreId",
                table: "StoreMaterial",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitMaterialDiscount_Unit_UnitId",
                table: "UnitMaterialDiscount",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ColumnInfo_ModuleInfo_ModuleInfoId",
                table: "ColumnInfo",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_CreatorUserId",
                table: "ModuleButton",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_DeleterUserId",
                table: "ModuleButton",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_LastModifierUserId",
                table: "ModuleButton",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_ModuleInfo_ModuleInfoId",
                table: "ModuleButton",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_CreatorUserId",
                table: "ModuleData",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_DeleterUserId",
                table: "ModuleData",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_LastModifierUserId",
                table: "ModuleData",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_ModuleInfo_ModuleInfoId",
                table: "ModuleData",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_CreatorUserId",
                table: "Project",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_DeleterUserId",
                table: "Project",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_LastModifierUserId",
                table: "Project",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Unit_UnitId",
                table: "Project",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Order_OrderId",
                table: "Project",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organization_OrganizationId",
                table: "User",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_CreatorUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_DeleterUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_LastModifierUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_CreatorUserId",
                table: "Tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_DeleterUserId",
                table: "Tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_LastModifierUserId",
                table: "Tenant");

            migrationBuilder.DropTable(
                name: "ApplicationLanguage");

            migrationBuilder.DropTable(
                name: "ApplicationLanguageText");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "BaseType");

            migrationBuilder.DropTable(
                name: "ColumnInfo");

            migrationBuilder.DropTable(
                name: "Dictionary");

            migrationBuilder.DropTable(
                name: "FeatureSetting");

            migrationBuilder.DropTable(
                name: "FeeAccountHistory");

            migrationBuilder.DropTable(
                name: "FeeCheck");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "FlowInstanceOperationHistory");

            migrationBuilder.DropTable(
                name: "FlowInstanceTransitionHistory");

            migrationBuilder.DropTable(
                name: "FlowSheetContent");

            migrationBuilder.DropTable(
                name: "MaterialBuy");

            migrationBuilder.DropTable(
                name: "MaterialSell");

            migrationBuilder.DropTable(
                name: "MaterialSellCart");

            migrationBuilder.DropTable(
                name: "ModuleButton");

            migrationBuilder.DropTable(
                name: "ModuleData");

            migrationBuilder.DropTable(
                name: "Notice");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StoreMaterial");

            migrationBuilder.DropTable(
                name: "StoreMaterialHistory");

            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "UnitFeeHistory");

            migrationBuilder.DropTable(
                name: "UnitMaterialDiscount");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserLoginAttempt");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "FeeAccount");

            migrationBuilder.DropTable(
                name: "ModuleInfo");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "FlowSheet");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "FlowInstance");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "BaseTree");

            migrationBuilder.DropTable(
                name: "FlowScheme");

            migrationBuilder.DropTable(
                name: "FlowForm");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "Edition");
        }
    }
}

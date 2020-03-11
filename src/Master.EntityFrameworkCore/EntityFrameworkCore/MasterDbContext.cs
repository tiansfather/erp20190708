using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Localization;
using Master.Application;
using Master.Application.Editions;
using Master.Application.Features;
using Master.Auditing;
using Master.Authentication;
using Master.Configuration;
using Master.Configuration.Dictionaries;
using Master.Entity;
using Master.EntityFrameworkCore.Repositories;
using Master.Finance;
using Master.Module;
using Master.MultiTenancy;
using Master.Notices;
using Master.Orders;
using Master.Organizations;
using Master.Projects;
using Master.Resources;
using Master.Storage;
using Master.Templates;
using Master.Units;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Master.EntityFrameworkCore
{
    [AutoRepositoryTypes(
    typeof(IRepository<>),
    typeof(IRepository<,>),
    typeof(MasterRepositoryBase<>),
    typeof(MasterRepositoryBase<,>)
    )]
    public class MasterDbContext : MasterDbContextBase
    {
        #region Entities
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<Edition> Edition { get; set; }
        public virtual DbSet<ApplicationLanguage> ApplicationLanguage { get; set; }
        public virtual DbSet<ApplicationLanguageText> ApplicationLanguageText { get; set; }
        public virtual DbSet<FeatureSetting> FeatureSetting { get; set; }
        public virtual DbSet<EditionFeatureSetting> EditionFeatureSetting { get; set; }
        public virtual DbSet<TenantFeatureSetting> TenantFeatureSetting { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
        public virtual DbSet<UserLoginAttempt> UserLoginAttempt { get; set; }
        public virtual DbSet<PermissionSetting> Permission { get; set; }
        public virtual DbSet<RolePermissionSetting> RolePermission { get; set; }
        public virtual DbSet<UserPermissionSetting> UserPermission { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }        
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ModuleInfo> ModuleInfo { get; set; }
        public virtual DbSet<ModuleData> ModuleData { get; set; }
        public virtual DbSet<ModuleButton> ModuleButton { get; set; }
        public virtual DbSet<ColumnInfo> ColumnInfo { get; set; }
        public virtual DbSet<Dictionary> Dictionary { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<BaseTree> BaseTree { get; set; }
        public virtual DbSet<BaseType> BaseType { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        #endregion

        #region WorkFlow
        public DbSet<FlowScheme> FlowScheme { get; set; }
        public DbSet<FlowInstance> FlowInstance { get; set; }
        public DbSet<FlowForm> FlowForm { get; set; }
        public DbSet<FlowSheet> FlowSheet { get; set; }
        public DbSet<FlowInstanceOperationHistory> FlowInstanceOperationHistory { get; set; }
        public DbSet<FlowInstanceTransitionHistory> FlowInstanceTransitionHistory { get; set; }
        #endregion

        #region Storage
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialBuy> MaterialBuy { get; set; }
        public virtual DbSet<MaterialBuyBack> MaterialBuyBack { get; set; }
        public virtual DbSet<StoreMaterial> StoreMaterial { get; set; }
        public virtual DbSet<StoreMaterialHistory> StoreMaterialHistory { get; set; }       
        public virtual DbSet<UnitMaterialDiscount> UnitMaterialDiscount { get; set; }
        public virtual DbSet<MaterialSell> MaterialSell { get; set; }
        public virtual DbSet<MaterialSellOut> MaterialSellOut { get; set; }
        public virtual DbSet<MaterialSellBack> MaterialSellBack { get; set; }
        public virtual DbSet<MaterialSellCart> MaterialSellCart { get; set; }
             
        #endregion

        #region Finance
        public virtual DbSet<FeeAccount> FeeAccount { get; set; }
        public virtual DbSet<FeeAccountHistory> FeeAccountHistory { get; set; }
        public virtual DbSet<UnitFeeHistory> UnitFeeHistory { get; set; }
        public virtual DbSet<FeeCheck> FeeCheck { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        #endregion


        public MasterDbContext(DbContextOptions<MasterDbContext> options) 
            : base(options)
        {

        }

        #region DbFunction
        [DbFunction(FunctionName = "json_extract")]
        public static string GetJsonValueString(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return "";
        }
        [DbFunction(FunctionName = "json_extract")]
        public static decimal? GetJsonValueNumber(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return 0;
        }
        [DbFunction(FunctionName = "json_extract")]
        public static DateTime GetJsonValueDate(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return DateTime.Now;
        }
        [DbFunction(FunctionName = "json_extract")]
        public static bool GetJsonValueBool(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return true;
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///动态加入实体
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes().Where(o => typeof(IAutoEntity).IsAssignableFrom(o) && o.IsClass && !o.IsAbstract))
                {
                    modelBuilder.Model.GetOrAddEntityType(type);
                }
                //通过反射加入实体配置
                modelBuilder.AddEntityConfigurationsFromAssembly(asm);
            }

            base.OnModelCreating(modelBuilder);

            
        }
    }
}

using Abp.EntityFrameworkCore;
using Master.Authentication;
using Master.MultiTenancy;
using Master.Projects;
using Master.Units;
using Master.WorkFlow.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore
{
    public class WorkFlowDbContext : MasterDbContextBase
    {

        public WorkFlowDbContext(DbContextOptions<WorkFlowDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<FlowScheme> FlowScheme { get; set; }
        public DbSet<FlowInstance> FlowInstance { get; set; }
        public DbSet<FlowForm> FlowForm { get; set; }
        public DbSet<FlowSheet> FlowSheet { get; set; }
        public DbSet<FlowInstanceOperationHistory> FlowInstanceOperationHistory { get; set; }
        public DbSet<FlowInstanceTransitionHistory> FlowInstanceTransitionHistory { get; set; }

        #region DbFunction
        [DbFunction(FunctionName = "json_extract")]
        public static string GetJsonValueString(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
        {
            return "";
        }
        [DbFunction(FunctionName = "json_extract")]
        public static decimal GetJsonValueNumber(JsonObject<IDictionary<string, object>> obj, string PropertyPath)
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
            
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {                
                //通过反射加入实体配置
                modelBuilder.AddEntityConfigurationsFromAssembly(asm);
            }

            base.OnModelCreating(modelBuilder);


        }
    }
}

using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;
using Master.Configuration;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed;
using System;
using System.Reflection;
using Abp.Configuration.Startup;
using Master.Entity;
using System.Linq;

namespace Master
{
    [DependsOn(
        typeof(MasterCoreModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule))]
    public class MasterWorkFlowModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpEfCore().AddDbContext<WorkFlowDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(MasterWorkFlowModule).GetAssembly(),"workflow"
                 );

            Configuration.Settings.Providers.Add<WorkFlowSettingProvider>();
            Configuration.Features.Providers.Add<WorkFlowFeatureProvider>();

            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Views/",
                    Assembly.GetExecutingAssembly(),
                    "Master.Views"
                )
            );
            //dto映射配置
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            //{
            //    config.CreateMap<SchedulerTask, ProjectSchedulerTaskDto>()
            //          .ForMember(u => u.Persons, options =>
            //          {
            //              options.MapFrom(input => string.Join(',', input.SchedulerTaskPeople.Where(o=>o.RelativeType==SchedulerTaskPersonRelativeType.Charger).Select(o => o.User.Name)));
            //          })
            //          .ForMember(u => u.RelPersons, options =>
            //          {
            //              options.MapFrom(input => string.Join(',', input.SchedulerTaskPeople.Where(o => o.RelativeType == SchedulerTaskPersonRelativeType.Relative).Select(o => o.User.Name)));
            //          });
            //});

            IocManager.Register<WorkFlowConfiguration>();
            //模块相关设置
            //加入通用模板视图
            //Configuration.Modules.WebCore().CommonViews.Add("../MES/Common");
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterWorkFlowModule).GetAssembly());
        }

        public override void PostInitialize()
        {

            using (var migratorWrapper = IocManager.ResolveAsDisposable<WorkFlowDbMigrator>())
            {
                migratorWrapper.Object.CreateOrMigrateForHost();
            }
        }

    }
}

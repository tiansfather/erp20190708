using Abp.Collections.Extensions;
using Abp.Modules;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Master.Module;

namespace Master.WorkFlow
{
    public class FlowSheetManager : ModuleServiceBase<FlowSheet, int>, IFlowSheetManager
    {
        public IAbpModuleManager AbpModuleManager { get; set; }
        //public IFlowHandlerFinder SheetHandlerFinder { get; set; }
        /// <summary>
        /// 获取单据模板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<string> GetSheetTemplateByName(string name)
        {
            var key = name + "@" + AbpSession.TenantId ?? "0";
            return CacheManager.GetCache<string, string>("SheetTemplate").Get(key, s => {
                var modules = AbpModuleManager.Modules;
                //按依赖对所有模块排序，保证后层模块最先调用
                var sortedModules = modules.SortByDependencies(o => o.Dependencies);
                sortedModules.Reverse();
                var templateContent = "";
                foreach (var module in sortedModules)
                {
                    //读取嵌入资源
                    templateContent = Common.Fun.ReadEmbedString(module.Assembly, "Template." + name + ".html");
                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        break;
                    }
                }

                return templateContent;
            });
            
        }

        public override async Task FillEntityDataBefore(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var sheet = entity as FlowSheet;
            data["Status"] = sheet.Status;
        }

    }
}

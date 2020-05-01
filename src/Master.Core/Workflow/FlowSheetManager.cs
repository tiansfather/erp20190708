using Abp.Collections.Extensions;
using Abp.Modules;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Abp.UI;

namespace Master.WorkFlow
{
    public class FlowSheetManager : ModuleServiceBase<FlowSheet, int>, IFlowSheetManager
    {
        private static object _obj = new object();
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
        public virtual async Task<FlowSheet> GetByInstanceId(int instanceId)
        {
            return await GetAll().Include(o=>o.FlowInstance).ThenInclude(o=>o.FlowForm).Where(o => o.FlowInstanceId == instanceId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ModuleButton>> GetFlowBtns(int sheetId)
        {
            var flowInstanceManager = Resolve<FlowInstanceManager>();
            var flowSheet = await GetAll().Include(o => o.FlowInstance).ThenInclude(o => o.FlowForm).Where(o => o.Id == sheetId).SingleOrDefaultAsync();
            if (flowSheet == null)
            {
                throw new UserFriendlyException($"找不到对应的单据,Id:{sheetId}");
            }
            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(flowSheet.FormKey);
            return await flowHandler.GetFlowBtns(flowSheet);
        }
        public virtual async Task Action(int sheetId, string action,string formData="",DateTime? lastModifyTime=null)
        {
            var flowInstanceManager = Resolve<FlowInstanceManager>();
            var flowSheet = await GetAll()
                .Include(o => o.FlowInstance).ThenInclude(o => o.FlowForm)
                .Include(o=>o.FlowInstance).ThenInclude(o=>o.LastModifierUser)
                .Where(o => o.Id == sheetId).SingleOrDefaultAsync();
            if (!string.IsNullOrEmpty(formData))
            {
                flowSheet.FlowInstance.FormData = formData;
            }
            
            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(flowSheet.FormKey);
            await flowHandler.Action(flowSheet, action, lastModifyTime);
        }
        public virtual async Task Revert(int sheetId,string revertReason)
        {
            var flowInstanceManager = Resolve<FlowInstanceManager>();
            var flowSheet = await GetAll().Include(o => o.FlowInstance).ThenInclude(o=>o.FlowForm).Where(o => o.Id == sheetId).SingleOrDefaultAsync();
            ////产生新的流程实例
            //var instance = flowSheet.FlowInstance;
            //var newInstance = new FlowInstance()
            //{
            //    InstanceName = instance.InstanceName,
            //    FormContent = instance.FormContent,
            //    FormData = instance.FormData,
            //    FlowFormId = instance.FlowFormId,
            //    FormType = instance.FormType,
            //    InstanceStatus = instance.InstanceStatus,
            //    IsActive = true,
            //    Code = Common.Fun.ConvertToTimeStamp(DateTime.Now).ToString()
            //};
            //var newInstanceId = await flowInstanceManager.InsertAndGetIdAsync(newInstance);
            ////生成新的单据
            //var newFlowSheet = new FlowSheet()
            //{
            //    FlowInstanceId = newInstanceId,
            //    SheetSN = GenerateSheetSN(flowSheet.FormKey),
            //    SheetName = newInstance.InstanceName,
            //    FormKey = flowSheet.FormKey,
            //    SheetNature = SheetNature.冲红,
            //    RelSheetId = flowSheet.Id,
            //    RevertReason = revertReason
            //};
            //newFlowSheet.SheetDate = flowSheet.SheetDate;
            //newFlowSheet.Remarks = flowSheet.Remarks;
            //var newSheetId = await InsertAndGetIdAsync(newFlowSheet);
            ////设置旧单据状态
            //flowSheet.SheetNature = SheetNature.被冲红;
            //flowSheet.RelSheetId = newSheetId;
            //flowSheet.RevertReason = revertReason;
            
            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(flowSheet.FormKey);

            await flowHandler.CreateRevertSheet(flowSheet, revertReason);
            //await flowHandler.HandleRevert(newFlowSheet);
        }

        public override async Task FillEntityDataBefore(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var sheet = entity as FlowSheet;
            data["Status"] = sheet.Status;
        }

        public virtual string GenerateSheetSN(string formKey)
        {
            lock (_obj)
            {
                //当天数量
                var todayNum = GetAll().IgnoreQueryFilters().Where(o => o.FormKey == formKey && o.CreationTime.Year == DateTime.Now.Year && o.CreationTime.Month == DateTime.Now.Month && o.CreationTime.Day == DateTime.Now.Day && !string.IsNullOrEmpty(o.SheetSN)).Count();
                var newNum = todayNum + 1;

                var sheetSN = formKey + DateTime.Now.ToString("yyyyMMdd") + newNum.ToString().PadLeft(4, '0');
                return sheetSN;
            }
                
        }
    }
}

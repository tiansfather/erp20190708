using Abp.Authorization;
using Master.WorkFlow.Modules;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Master.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace Master.FlowSheets
{
    [AbpAuthorize]
    public class SDRSheetAppService : ModuleDataAppServiceBase<FlowSheet, int>
    {
        protected override async Task<IQueryable<FlowSheet>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Where(o=>o.FormKey=="SDR");
        }
        protected override string ModuleKey()
        {
            return nameof(SDRSheet);
        }

        /// <summary>
        /// 批量审校
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Verify(IEnumerable<int> ids)
        {
            var manager = Manager as FlowSheetManager;
            foreach(var id in ids)
            {
                await manager.Action(id, "verify");

            }
            
        }
        /// <summary>
        /// 批量取消
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Cancel(IEnumerable<int> ids)
        {
            var manager = Manager as FlowSheetManager;
            foreach (var id in ids)
            {
                await manager.Action(id, "cancel");

            }

        }
    }
}

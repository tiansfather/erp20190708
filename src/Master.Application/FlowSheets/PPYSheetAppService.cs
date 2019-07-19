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
    public class PPYSheetAppService : ModuleDataAppServiceBase<FlowSheet, int>
    {
        protected override async Task<IQueryable<FlowSheet>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Where(o=>o.FormKey=="PPY");
        }
        protected override string ModuleKey()
        {
            return nameof(PPYSheet);
        }
    }
}

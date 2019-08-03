using Abp.Authorization;
using Master.Dto;
using Master.WorkFlow;
using Master.WorkFlow.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowSheets
{
    [AbpAuthorize]
    public class ICHSheetAppService : FlowSheetModuleAppServiceBase
    {
        protected override async Task<IQueryable<FlowSheet>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Where(o => o.FormKey == "ICH");
        }
        protected override string ModuleKey()
        {
            return nameof(ICHSheet);
        }
    }
}

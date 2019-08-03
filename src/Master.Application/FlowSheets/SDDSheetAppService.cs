using Abp.Authorization;
using Master.WorkFlow.Modules;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Master.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;

namespace Master.FlowSheets
{
    [AbpAuthorize]
    public class SDDSheetAppService : FlowSheetModuleAppServiceBase
    {
        protected override async Task<IQueryable<FlowSheet>> GetQueryable(RequestPageDto request)
        {
            var user = await GetCurrentUserAsync();
            return (await base.GetQueryable(request))
                .WhereIf(user.UnitId.HasValue,o=>o.UnitId==user.UnitId)//代理商登录只看到自己
                .Where(o=>o.FormKey=="SDD");
        }
        protected override string ModuleKey()
        {
            return nameof(SDDSheet);
        }
    }
}

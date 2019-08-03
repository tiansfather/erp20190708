using Abp.Linq.Extensions;
using Master.Dto;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowSheets
{
    public class FlowSheetModuleAppServiceBase : ModuleDataAppServiceBase<FlowSheet, int>
    {
        protected override async Task<IQueryable<FlowSheet>> GetQueryable(RequestPageDto request)
        {
            var user = await GetCurrentUserAsync();
            return (await base.GetQueryable(request))
                .WhereIf(user.UnitId.HasValue, o => o.UnitId == user.UnitId)//代理商登录只看到自己;
                ;
        }

        protected override string ModuleKey()
        {
            throw new NotImplementedException();
        }
    }
}

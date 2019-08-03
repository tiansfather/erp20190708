using Abp.Linq.Extensions;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class UnitFeeHistoryAppService : ModuleDataAppServiceBase<UnitFeeHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(UnitFeeHistory);
        }

        protected override async Task<IQueryable<UnitFeeHistory>> GetQueryable(RequestPageDto request)
        {
            var user = await GetCurrentUserAsync();
            return (await base.GetQueryable(request))
                .Include(o => o.FlowSheet)
                 .WhereIf(user.UnitId.HasValue, o => o.FlowSheet.UnitId == user.UnitId)//代理商登录只看到自己;
                ;
        }
    }
}

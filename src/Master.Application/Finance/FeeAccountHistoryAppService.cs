using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class FeeAccountHistoryAppService : ModuleDataAppServiceBase<FeeAccountHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(FeeAccountHistory);
        }
        public override async Task<Dictionary<string, object>> GetPageSummary(IQueryable<FeeAccountHistory> queryable, RequestPageDto requestPageDto)
        {
            var result = new Dictionary<string, object>();
            result.Add("发生金额", (await queryable.SumAsync(o => o.Fee)).ToString("0.00"));
            return result;
        }
    }
}

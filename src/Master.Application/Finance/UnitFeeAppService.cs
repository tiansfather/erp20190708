using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Master.Dto;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    [AbpAuthorize]
    public class UnitFeeAppService:MasterAppServiceBase<UnitFeeHistory,int>
    {
        public virtual async Task<object> GetUnitFeeSummary(int? unitId,int? unitNature,DateTime? endDate)
        {
            var query = Manager.GetAll()
                .Include(o=>o.Unit)
                .Include(o=>o.FlowSheet)
                .Where(o=>o.RemainFee!=0)
                .WhereIf(unitId.HasValue, o => o.UnitId == unitId)
                .WhereIf(unitNature.HasValue, o => o.Unit.UnitNature == (UnitNature)unitNature.Value)
                .WhereIf(endDate.HasValue, o => o.CreationTime < endDate.Value.AddDays(1));

            var data = (await query.GroupBy(o => o.Unit)
                .Select(o => new
                {
                    o.Key.UnitName,
                    LastHistory=o.OrderByDescending(f=>f.CreationTime).FirstOrDefault()
                })
                .ToListAsync())
                .Select(o=>new
                {
                    o.UnitName,
                    o.LastHistory?.RemainFee,
                    o.LastHistory?.FlowSheet?.SheetSN,
                    CreationTime=o.LastHistory?.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
                })
                ;

            return data;
        }
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var pageResult = await GetPageResultQueryable(request);

            var units = await pageResult.Queryable.ToListAsync();

            var data = new List<object>();
            var endDate = DateTime.Now;
            if (!request.SearchKeys.IsNullOrWhiteSpace())
            {
                var searchKeys = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(request.SearchKeys);
                if (searchKeys.ContainsKey("EndDate"))
                {
                    endDate = Convert.ToDateTime(searchKeys["EndDate"]);
                }
            }
            foreach (var unit in units)
            {

            }

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }
    }
}

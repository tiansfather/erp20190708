using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialBuyAppService:MasterAppServiceBase<MaterialBuy,int>
    {
        public virtual async Task<object> GetUnitBuyedMaterial(int unitId,DateTime startDate)
        {
            return await Manager.GetAll().Include(o => o.FlowSheet).Include(o => o.Material)
                .Where(new MaterialBuySpecification(unitId,startDate))
                .GroupBy(o => o.Material)
                .Select(o => new
                {
                    o.Key.Id,
                    o.Key.Name,
                    o.Key.Specification,
                    CanBackNumber=o.Sum(b=>b.CanBackNumber)
                })
                .Where(o=>o.CanBackNumber>0)
                .ToListAsync();
        }
    }
}

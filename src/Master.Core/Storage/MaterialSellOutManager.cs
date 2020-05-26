using Abp.Linq.Extensions;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialSellOutManager : DomainServiceBase<MaterialSellOut, int>
    {
        /// <summary>
        /// 获取代理商时间段内的出库金额
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetUnitSellOutFee(int unitId,DateTime? startDate,DateTime? endDate)
        {
            return await GetAll().Where(o => o.UnitId == unitId)
                .WhereIf(startDate.HasValue, o => o.CreationTime >= startDate.Value)
                .WhereIf(endDate.HasValue, o => o.CreationTime <= endDate.Value)
                .SumAsync(o => o.Price * o.Discount);
        }
    }
}

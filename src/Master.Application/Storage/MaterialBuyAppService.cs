using Abp.Authorization;
using Abp.UI;
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
                    o.Key.Price,
                    BuyNumber=o.Sum(b=>b.BuyNumber),
                    BackNumber=o.Sum(b=>b.BackNumber),
                    CanBackNumber=o.Sum(b=>b.CanBackNumber)
                })
                .Where(o=>o.CanBackNumber>0)
                .ToListAsync();
        }
        /// <summary>
        /// 卡号检查
        /// </summary>
        /// <param name="featureCode"></param>
        /// <param name="codeNumberStr"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckCode(string featureCode,string codeNumberStr)
        {
            long codeNumber;
            if(!long.TryParse(codeNumberStr, out codeNumber))
            {
                throw new UserFriendlyException("卡号必须为数字类型");
            }
            var count=await Manager.GetAll().FromSql($"select * from materialbuy where FeatureCode='{featureCode}' and CAST(CodeStartNumber AS Decimal(24))<{codeNumber} and CAST(CodeEndNumber AS Decimal(24))>{codeNumber}").CountAsync();
            return count>0;
        }
    }
}

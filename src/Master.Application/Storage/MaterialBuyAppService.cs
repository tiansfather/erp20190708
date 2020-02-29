using Abp.Authorization;
using Abp.UI;
using Master.Domain;
using Master.Dto;
using Master.Entity;
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
        public BaseTreeManager BaseTreeManager { get; set; }
        protected override async Task<IQueryable<MaterialBuy>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o => o.FlowSheet)
                .Include(o => o.Material).ThenInclude(o => o.MaterialType)
                .Where(o=>o.FlowSheet.SheetNature==WorkFlow.SheetNature.正单)
                ;
        }
        protected override async Task<IQueryable<MaterialBuy>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<MaterialBuy> query)
        {

            if (searchKeys.ContainsKey("MaterialTypeId"))
            {
                if (int.TryParse(searchKeys["MaterialTypeId"], out var materialTypeId))
                {
                    if (materialTypeId == -1)
                    {
                        query = query.Where(o => o.Material.MaterialTypeId == null);
                    }
                    else
                    {
                        var materialType = await BaseTreeManager.GetByIdFromCacheAsync(materialTypeId);
                        //所有子类的id集合
                        var childIds = (await BaseTreeManager.FindChildrenAsync(materialTypeId, materialType.Discriminator, true)).Select(o => o.Id);
                        query = query.Where(o => o.Material.MaterialTypeId == materialTypeId || o.Material.MaterialTypeId != null && childIds.Contains(o.Material.MaterialTypeId.Value));
                    }

                }
            }
            if (searchKeys.ContainsKey("startDate"))
            {
                query = query.Where(o => o.CreationTime > DateTime.Parse(searchKeys["startDate"]));
            }
            if (searchKeys.ContainsKey("endDate"))
            {
                query = query.Where(o => o.CreationTime < DateTime.Parse(searchKeys["endDate"]));
            }
            return query;
        }
        protected override object PageResultConverter(MaterialBuy entity)
        {
            return new
            {
                entity.FlowSheet.SheetSN,
                entity.Material.Name,
                MaterialTypeName = entity.Material.MaterialType.DisplayName,
                UnitName = entity.Unit.UnitName,
                entity.Material.Specification,
                MaterialNature = entity.Material.MaterialNature.ToString(),
                entity.Price,
                entity.Discount,
                entity.Material.MeasureMentUnit,
                entity.BuyNumber
            };
        }

        public virtual async Task<object> GetUnitBuyedMaterial(int unitId,int storeId,DateTime startDate)
        {
            var storeMaterialManager = Resolve<StoreMaterialManager>();
            return (await Manager.GetAll().Include(o => o.FlowSheet).Include(o => o.Material)
                .Where(new MaterialBuySpecification(unitId,startDate))
                .GroupBy(o => o.Material)
                .Select(o => new
                {
                    o.Key.Id,
                    o.Key.Name,
                    o.Key.Specification,
                    o.Key.Price,
                    Discount=o.Key.DefaultBuyDiscount??1,
                    BuyNumber=o.Sum(b=>b.BuyNumber),
                    BackNumber=o.Sum(b=>b.BackNumber),
                    CanBackNumber=o.Sum(b=>b.CanBackNumber),                    
                })
                .Where(o=>o.CanBackNumber>0)
                .ToListAsync())
                .Select(o=>new
                {
                    o.Id,
                    o.Name,
                    o.Specification,
                    o.Price,
                    o.Discount,
                    o.BuyNumber,
                    o.BackNumber,
                    o.CanBackNumber,
                    StoreNumber = storeMaterialManager.GetStoreMaterialNumber(storeId, o.Id).Result
                });
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
            var count = await Resolve<IDynamicQuery>().FirstOrDefaultAsync<int>($"select count(0) from materialbuy where FeatureCode='{featureCode}' and CAST(CodeStartNumber AS Decimal(24))<={codeNumber} and CAST(CodeEndNumber AS Decimal(24))>={codeNumber}");
            //var count=await Manager.GetAll().FromSql($"select * from materialbuy where FeatureCode='{featureCode}' and CAST(CodeStartNumber AS Decimal(24))<{codeNumber} and CAST(CodeEndNumber AS Decimal(24))>{codeNumber}").CountAsync();
            return count>0;
        }
    }
}

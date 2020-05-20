using Abp.Authorization;
using Abp.UI;
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
    public class MaterialBuyBackAppService:MasterAppServiceBase<MaterialBuyBack,int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        protected override async Task<IQueryable<MaterialBuyBack>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o => o.FlowSheet)
                .Include(o => o.Material).ThenInclude(o => o.MaterialType)
                .Where(o=>o.FlowSheet.SheetNature==WorkFlow.SheetNature.正单)
                ;
        }
        protected override async Task<IQueryable<MaterialBuyBack>> BuildKeywordQueryAsync(string keyword, IQueryable<MaterialBuyBack> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.Material.MaterialType.DisplayName.Contains(keyword) || o.Material.Name.Contains(keyword) || o.FlowSheet.SheetSN.Contains(keyword));
        }
        protected override async Task<IQueryable<MaterialBuyBack>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<MaterialBuyBack> query)
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
        protected override object PageResultConverter(MaterialBuyBack entity)
        {
            return new
            {
                entity.FlowSheet?.SheetSN,
                entity.Material?.Name,
                MaterialTypeName = entity.Material?.MaterialType?.DisplayName,
                UnitName = entity.Unit?.UnitName,
                entity.Material?.Specification,
                MaterialNature = entity.Material?.MaterialNature.ToString(),
                entity.Price,
                entity.Discount,
                entity.Material?.MeasureMentUnit,
                entity.BackNumber,
                CreationTime = entity.CreationTime.ToString("yyyy-MM-dd HH:mm")
            };
        }

    }
}

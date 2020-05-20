using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialBuySummaryAppService:MasterAppServiceBase<MaterialBuy, int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public MaterialBuyManager MaterialBuyManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        #region 分页
        protected override async Task<IQueryable<MaterialBuy>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Include(o => o.FlowSheet)
                .Include(o => o.Material).ThenInclude(o => o.MaterialType)
                .Where(o => o.FlowSheet.SheetNature == WorkFlow.SheetNature.正单)
                ;
        }
        protected override async Task<IQueryable<MaterialBuy>> BuildKeywordQueryAsync(string keyword, IQueryable<MaterialBuy> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.Material.MaterialType.DisplayName.Contains(keyword) || o.Material.Name.Contains(keyword));
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
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var query = await GetQueryable(request);

            
            var pageResult = query.GroupBy(o => new { o.Material, o.Unit }).PageResult(request.Page, request.Limit);

            var groupResult =await pageResult.Queryable.ToListAsync();

            var data = new List<object>();
            foreach(var group in groupResult)
            {
                var material = group.Key.Material;
                var unit = group.Key.Unit;
                //获取入库总数量
                var buyNumber = group.Sum(o => o.BuyNumber - o.BackNumber);
                var fee = group.Sum(o => o.Price * o.Discount * (o.BuyNumber - o.BackNumber));
                data.Add(new
                {
                    material.Name,
                    unit.UnitName,
                    MaterialTypeName = material.MaterialType?.DisplayName,
                    material.Specification,
                    material.MeasureMentUnit,
                    MaterialNature=material.MaterialNature.ToString(),
                    buyNumber,
                    fee,
                });
            }

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }

        #endregion

        
    }
}

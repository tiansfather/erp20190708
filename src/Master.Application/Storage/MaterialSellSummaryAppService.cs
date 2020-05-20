using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Web.Models;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialSellSummaryAppService:MasterAppServiceBase<Material,int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public MaterialSellManager MaterialSellManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        #region 分页
        protected override async Task<IQueryable<Material>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);
            query=query.Include(o=>o.MaterialType)
                ;
            var startDate = DateTime.Parse("2019-01-01");
            var endDate = DateTime.Now;

            var searchKeyDic = request.SearchKeyDic;
            if (searchKeyDic.ContainsKey("startDate"))
            {
                startDate = DateTime.Parse(searchKeyDic["startDate"]);
            }
            if (searchKeyDic.ContainsKey("endDate"))
            {
                endDate = DateTime.Parse(searchKeyDic["endDate"]);
            }
            var summary = await GetSummary(startDate, endDate);
            var summaryIdWithValues = summary.Where(o => o.SellNumber != 0).Select(o => o.Id);
            query = query.Where(o => summaryIdWithValues.Contains(o.Id));

            return query;
        }
        protected override async Task<IQueryable<Material>> BuildKeywordQueryAsync(string keyword, IQueryable<Material> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.Name.Contains(keyword) || o.MaterialType.DisplayName.Contains(keyword));
        }
        protected override async Task<IQueryable<Material>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Material> query)
        {
            
            if (searchKeys.ContainsKey("MaterialTypeId"))
            {
                if (int.TryParse(searchKeys["MaterialTypeId"], out var materialTypeId))
                {
                    if (materialTypeId == -1)
                    {
                        query = query.Where(o => o.MaterialTypeId == null);
                    }
                    else
                    {
                        var materialType = await BaseTreeManager.GetByIdFromCacheAsync(materialTypeId);
                        //所有子类的id集合
                        var childIds = (await BaseTreeManager.FindChildrenAsync(materialTypeId, materialType.Discriminator, true)).Select(o => o.Id);
                        query = query.Where(o => o.MaterialTypeId == materialTypeId || o.MaterialTypeId != null && childIds.Contains(o.MaterialTypeId.Value));
                    }

                }
            }
            return query;
        }
        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            
            var pageResult = await GetPageResultQueryable(request);

            var materials = await pageResult.Queryable.ToListAsync();
            
            var data = new List<object>();
            foreach(var material in materials)
            {
                //获取订货总数量及库存数量
                //var sellNumber = await baseQuery.Where(o => o.MaterialId == material.Id).SumAsync(o => o.SellNumber);
                //var outNumber = await baseQuery.Where(o => o.MaterialId == material.Id).SumAsync(o => o.OutNumber);
                //var storeNumber = await StoreMaterialManager.GetAll().Where(o => o.MaterialId == material.Id).SumAsync(o => o.Number);
                //data.Add(new
                //{
                //    material.Name,
                //    MaterialTypeName = material.MaterialType?.DisplayName,
                //    material.Specification,
                //    material.MeasureMentUnit,
                //    MaterialNature=material.MaterialNature.ToString(),
                //    sellNumber,
                //    outNumber,
                //    storeNumber
                //});
                data.Add(_summarys.Single(o => o.Id == material.Id));
            }

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }

        private IEnumerable<MaterialSellSummaryDto> _summarys;
        private async Task<IEnumerable<MaterialSellSummaryDto>> GetSummary(DateTime startDate, DateTime endDate)
        {
            if (_summarys == null)
            {
                _summarys =await GetMaterialSellSummarysInner(startDate, endDate);
            }
            return _summarys;
        }
        private async Task<IEnumerable<MaterialSellSummaryDto>> GetMaterialSellSummarysInner(DateTime startDate,DateTime endDate)
        {
            var result = new List<MaterialSellSummaryDto>();
            var user = await GetCurrentUserAsync();
            var baseQuery = MaterialSellManager.GetAll()
                .Where(o => o.FlowSheet.SheetNature == WorkFlow.SheetNature.正单)
                .Where(o => o.FlowSheet.OrderStatus == null || (o.FlowSheet.OrderStatus != "待审核" && o.FlowSheet.OrderStatus != "已退货" && o.FlowSheet.OrderStatus != "已取消"))
                .Where(o => o.CreationTime > startDate && o.CreationTime < endDate)
                .WhereIf(user.UnitId.HasValue, o => o.FlowSheet.UnitId == user.UnitId)//代理商登录只看到自己;
                ;

            var materials = await Manager.GetAll().Include(o => o.MaterialType).ToListAsync();
            foreach (var material in materials)
            {
                //获取订货总数量及库存数量
                var sellNumber = await baseQuery.Where(o => o.MaterialId == material.Id).SumAsync(o => o.SellNumber);
                var outNumber = await baseQuery.Where(o => o.MaterialId == material.Id).SumAsync(o => o.OutNumber);
                //todo:需要减掉退货数量
                var materialSellBackQuery = Resolve<MaterialSellBackManager>().GetAll().Where(o => o.MaterialId == material.Id )
                    .Where(o => o.CreationTime >= startDate)
                    .Where(o => o.CreationTime < endDate);
                var backNumber = materialSellBackQuery.Sum(o => o.BackNumber);
                var storeNumber = await StoreMaterialManager.GetAll().Where(o => o.MaterialId == material.Id).SumAsync(o => o.Number);
                result.Add(new MaterialSellSummaryDto()
                {
                    Id=material.Id,
                    Name=material.Name,
                    MaterialTypeName = material.MaterialType?.DisplayName,
                    Specification=material.Specification,
                    MeasureMentUnit=material.MeasureMentUnit,
                    MaterialNature = material.MaterialNature.ToString(),
                    SellNumber=sellNumber,
                    OutNumber=outNumber-backNumber,
                    StoreNumber=storeNumber
                });
            }

            return result;
        }

        #endregion

        
    }
}

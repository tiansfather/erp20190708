using Abp.Authorization;
using Abp.Domain.Repositories;
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
    public class MaterialAppService : ModuleDataAppServiceBase<Material, int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public IRepository<UnitMaterialDiscount, int> UnitMaterialRepository { get; set; }
        protected override string ModuleKey()
        {
            return nameof(Material);
        }
        protected override async Task<IQueryable<Material>> BuildKeywordQueryAsync(string keyword, IQueryable<Material> query)
        {
            query = query.Where(o => o.Name.Contains(keyword) || o.Specification.Contains(keyword));
            return query;
        }
        protected override async Task<IQueryable<Material>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Material> query)
        {
            if (searchKeys.ContainsKey("From"))
            {
                //如果是用于销售的只显示上架商品
                if (searchKeys["From"] == "sell")
                {
                    query = query.Where(o => o.IsActive);
                }
            }
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
            if (searchKeys.ContainsKey("SellUnitId") && !string.IsNullOrEmpty(searchKeys["SellUnitId"]))
            {
                //如果按照代理来查询，则需要按代理的销售方式进行查询
                var _unitId = int.Parse(searchKeys["SellUnitId"]);
                query = query.Where(new MaterialSellUnitSpecification(_unitId));
                /*query=query.Where(o => 
                UnitMaterialRepository.GetAll().Count(d => d.UnitId == _unitId && d.MaterialId == o.Id &&( d.UnitSellMode==UnitSellMode.停止销售 || d.UnitSellMode == UnitSellMode.售完为止 && o.TotalNumber==0) ) == 0 //并未设置代理商销售方式的默认始终销售
                    );*/
            }
            return query;
        }

        /// <summary>
        /// 某物料的库存总量
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public virtual async Task<int> GetStoreMaterialCount(int materialId)
        {
            return await Resolve<StoreMaterialManager>().GetAll().CountAsync(o => o.MaterialId == materialId);
        }
        /// <summary>
        /// 获取物料信息，用于采购入库
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitMaterialInfos(int unitId,IEnumerable<int> materialIds)
        {
            var result = new List<object>();
            var materials = await Manager.GetListByIdsAsync(materialIds);
            foreach (var o in materials)
            {
                var obj = new
                {
                    o.Id,
                    o.Name,
                    o.Specification,
                    o.Remarks,
                    o.MeasureMentUnit,
                    o.MaterialNature,
                    o.Price,
                    o.Location,
                    o.DefaultBuyDiscount
                };
                result.Add(obj);
            }
            return result;
        }

        /// <summary>
        /// 获取某仓库中的物料信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        public virtual async Task<object> GetStoreMaterialInfos(int storeId,IEnumerable<int> materialIds)
        {
            var result = new List<object>();
            var materials = await Manager.GetListByIdsAsync(materialIds);
            var storeMaterialManager = Resolve<StoreMaterialManager>();
            foreach(var o in materials)
            {
                var storeMaterialNumber = await storeMaterialManager.GetStoreMaterialNumber(storeId, o.Id);
                var obj= new
                {
                    o.Id,
                    o.Name,
                    o.Specification,
                    o.Remarks,
                    o.MeasureMentUnit,
                    o.MaterialNature,
                    StoreNumber = storeMaterialNumber
                };
                result.Add(obj);
            }
            return result;
        }

        
    }
}

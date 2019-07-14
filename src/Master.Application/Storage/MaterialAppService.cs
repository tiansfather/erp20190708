using Abp.Authorization;
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
        protected override string ModuleKey()
        {
            return nameof(Material);
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

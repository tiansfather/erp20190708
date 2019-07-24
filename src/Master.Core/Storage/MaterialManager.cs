using Abp.Domain.Repositories;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialManager : ModuleServiceBase<Material, int>
    {
        public override async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var material = entity as Material;
            //if (moduleInfo.ModuleKey == nameof(MaterialDIY))
            //{
            //    data["IsDiyed"] = material.DIYInfo.Count > 0;
            //}
            //加入库存
            var storeCountInfo = await Resolve<StoreMaterialManager>().GetAll().Where(o => o.MaterialId == material.Id)
                .Select(o => new
                {
                    o.StoreId,
                    o.Number
                })
                .ToListAsync();
            data["StoreCount"] = storeCountInfo;
            data["TotalCount"] = storeCountInfo.Sum(o => o.Number);
        }
        /// <summary>
        /// 获取商品的供应商折扣
        /// </summary>
        /// <param name="material"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetMaterialUnitDiscount(Material material,int unitId)
        {
            decimal result = 1;
            var discountRepository = Resolve<IRepository<UnitMaterialDiscount, int>>();
            var disCount = await discountRepository.FirstOrDefaultAsync(o => o.MaterialId == material.Id && o.UnitId == unitId);
            if (disCount == null || disCount.UnitDiscount == UnitDiscount.默认)
            {
                result= material.DefaultSellDiscount ?? 1;
            }else if (disCount.UnitDiscount == UnitDiscount.折扣1)
            {
                result = material.SellDiscount1 ?? 1;
            }
            else if (disCount.UnitDiscount == UnitDiscount.折扣2)
            {
                result = material.SellDiscount1 ?? 1;
            }
            else if (disCount.UnitDiscount == UnitDiscount.折扣3)
            {
                result =  material.SellDiscount1 ?? 1;
            }
            return result;
        }
    }
}

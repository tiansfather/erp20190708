using Abp.Domain.Repositories;
using Abp.UI;
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
            var storeCountInfo = await Resolve<StoreMaterialManager>().GetAll().Include(o=>o.Store).Where(o => o.MaterialId == material.Id)
                .Select(o => new
                {
                    o.StoreId,
                    o.Store.IsDefault,
                    o.Number
                })
                .ToListAsync();
            data["StoreCount"] = storeCountInfo;
            data["DefaultCount"] = storeCountInfo.FirstOrDefault(o => o.IsDefault)?.Number;
            data["TotalCount"] = storeCountInfo.Sum(o => o.Number);
        }
        public override async Task ValidateEntity(Material entity)
        {
            if(!CheckDiscountNumberValid(entity.DefaultBuyDiscount) 
                || !CheckDiscountNumberValid(entity.DefaultSellDiscount) 
                || !CheckDiscountNumberValid(entity.SellDiscount1) 
                || !CheckDiscountNumberValid(entity.SellDiscount2)
                || !CheckDiscountNumberValid(entity.SellDiscount3))
            {
                throw new UserFriendlyException("折扣必须为0到1之间");
            }

            await base.ValidateEntity(entity);
        }

        private bool CheckDiscountNumberValid(decimal? DiscountNumber)
        {
            return DiscountNumber.HasValue && DiscountNumber.Value <= 1 && DiscountNumber.Value>=0 || !DiscountNumber.HasValue;
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
                result = material.SellDiscount2 ?? 1;
            }
            else if (disCount.UnitDiscount == UnitDiscount.折扣3)
            {
                result =  material.SellDiscount3 ?? 1;
            }
            return result;
        }
        /// <summary>
        /// 获取商品对供应商的销售方式
        /// </summary>
        /// <param name="material"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<UnitSellMode> GetMaterialUnitSellMode(Material material,int unitId)
        {
            var discountRepository = Resolve<IRepository<UnitMaterialDiscount, int>>();
            var disCount = await discountRepository.FirstOrDefaultAsync(o => o.MaterialId == material.Id && o.UnitId == unitId);
            if (disCount == null)
            {
                return UnitSellMode.始终销售;
            }
            return disCount.UnitSellMode;
        }
        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            var materialBuyCount = await Resolve<MaterialBuyManager>().GetAll().CountAsync(o => ids.Contains(o.MaterialId));
            var materialSellCount = await Resolve<MaterialSellManager>().GetAll().CountAsync(o => ids.Contains(o.MaterialId));
            var materialHistoryCount = await Resolve<StoreMaterialHistoryManager>().GetAll().CountAsync(o => ids.Contains(o.MaterialId));
            if(materialBuyCount>0 || materialSellCount>0 || materialHistoryCount > 0)
            {
                throw new UserFriendlyException("已使用过的商品不能删除");
            }
            await base.DeleteAsync(ids);
        }
    }
}

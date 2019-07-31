using Abp.UI;
using Master.Domain;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialBuyManager:DomainServiceBase<MaterialBuy,int>
    {
        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <param name="materialId"></param>
        /// <param name="storeId"></param>
        /// <param name="number"></param>
        /// <param name="flowSheet"></param>
        /// <returns></returns>
        public virtual async Task Back(int unitId,DateTime startDate,int materialId,int storeId,int number,FlowSheet flowSheet)
        {
            var material = await Resolve<MaterialManager>().GetByIdFromCacheAsync(materialId);

            var toDoNumber = number;
            var materialBuys = await GetAll().Where(new MaterialBuySpecification(unitId, startDate))
                .Where(o => o.MaterialId == materialId)
                .ToListAsync();

            foreach(var materialBuy in materialBuys)
            {
                if (toDoNumber == 0)
                {
                    break;
                }
                else
                {
                    if (materialBuy.CanBackNumber >= toDoNumber)
                    {
                        materialBuy.BackNumber += toDoNumber;
                        toDoNumber = 0;
                    }
                    else
                    {
                        toDoNumber -= materialBuy.CanBackNumber;
                        materialBuy.BackNumber = materialBuy.BuyNumber;
                    }
                }
                
            }

            if (toDoNumber > 0)
            {
                throw new UserFriendlyException($"{material.Name}可退货数量少于{toDoNumber}");
            }

            await Resolve<StoreMaterialManager>().CountMaterial(storeId, materialId, -number, flowSheet);
        }
    }
}

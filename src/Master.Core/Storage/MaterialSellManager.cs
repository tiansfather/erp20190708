using Abp.UI;
using Master.Domain;
using Master.Entity;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialSellManager:DomainServiceBase<MaterialSell,int>
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
        public virtual async Task Back(int unitId, DateTime startDate, int materialId, int storeId, int number, FlowSheet flowSheet)
        {
            var material = await Resolve<MaterialManager>().GetByIdFromCacheAsync(materialId);

            var toDoNumber = number;
            var materialSells = (await GetAll()
                .Include(o=>o.FlowSheet)
                .Where(new MaterialSellSpecification(unitId, startDate))
                .Where(o => o.SellNumber > o.BackNumber)//
                .Where(o => o.MaterialId == materialId)
                .ToListAsync())
                .Where(o=>o.FlowSheet.GetPropertyValue<string>("OrderStatus")!="待审核");

            foreach (var materialSell in materialSells)
            {
                if (toDoNumber == 0)
                {
                    break;
                }
                else
                {
                    if (materialSell.CanBackNumber >= toDoNumber)
                    {
                        materialSell.BackNumber += toDoNumber;
                        toDoNumber = 0;
                    }
                    else
                    {
                        toDoNumber -= materialSell.CanBackNumber;
                        materialSell.BackNumber = materialSell.SellNumber;
                    }
                }
            }

            if (toDoNumber > 0)
            {
                throw new UserFriendlyException($"{material.Name}可退货数量少于{toDoNumber}");
            }

            await Resolve<StoreMaterialManager>().CountMaterial(storeId, materialId, number, flowSheet);
        }
        /// <summary>
        /// 销售出货，此操作不改变库存
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="materialSellId"></param>
        /// <param name="storeId"></param>
        /// <param name="number"></param>
        /// <param name="flowSheet"></param>
        /// <returns></returns>
        public virtual async Task Out(int unitId, int materialSellId, int storeId, int number, FlowSheet flowSheet)
        {
            var materialSell = await GetByIdAsync(materialSellId);

            materialSell.OutNumber += number;
        }

        /// <summary>
        /// 检测订货单的状态：待出库、部分出库、出库完成
        /// </summary>
        /// <param name="flowSheetIds"></param>
        public virtual async Task CheckSellSheetStatus(IEnumerable<int> materialSellIds)
        {
            var flowSheetIds = await GetAll().Where(o => materialSellIds.Contains(o.Id)).GroupBy(o => o.FlowSheetId)
                .Select(o => o.Key).ToListAsync();
            var flowSheets = await Resolve<FlowSheetManager>().GetListByIdsAsync(flowSheetIds);
            foreach(var flowSheet in flowSheets)
            {
                var materialSells = await GetAll().Where(o => o.FlowSheetId == flowSheet.Id).ToListAsync();
                if (materialSells.Count(o => o.OutNumber > 0) == 0)
                {
                    flowSheet.OrderStatus= "待出库";
                }else if (materialSells.Count(o => o.OutNumber != o.SellNumber) == 0)
                {
                    flowSheet.OrderStatus="出库完成";
                }
                else
                {
                    flowSheet.OrderStatus="部分出库";
                }
            }
        }
    }
}

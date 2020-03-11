using Abp.UI;
using Master.Entity;
using Master.Extensions;
using Master.Storage;
using Master.Units;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowHandlers
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class PRHFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public MaterialBuyManager MaterialBuyManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public UnitManager UnitManager { get; set; }
        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var storeId = sheetHeader["storeId"].ToObject<int>();//调出仓库id;
            var unitId = sheetHeader["unitId"].ToObject<int>();//供货商id
            var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
            flowSheet.UnitId = unitId;
            flowSheet.SetPropertyValue("StoreName", sheetHeader["storeName"].ToObjectWithDefault<string>());
            flowSheet.SetPropertyValue("Fee", totalFee);

            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, totalFee, flowSheet);

            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//入库数量
                //记录采购
                var materialBuy = new MaterialBuy()
                {
                    UnitId=unitId,
                    MaterialId = materialId,
                    FlowSheetId = flowSheet.Id,
                    BuyNumber = number,
                    Price=sheetItem["price"].ToObjectWithDefault<decimal>(),
                    Discount=sheetItem["discount"].ToObjectWithDefault<decimal>(),
                    FeatureCode =sheetItem["featureCode"].ToObjectWithDefault<string>(),
                    CodeStartNumber= sheetItem["codeStartNumber"].ToObjectWithDefault<string>(),
                    CodeEndNumber = sheetItem["codeEndNumber"].ToObjectWithDefault<string>(),
                };
                await MaterialBuyManager.InsertAsync(materialBuy);
                //库存变化
                await StoreMaterialManager.CountMaterial(storeId, materialId, number,flowSheet);
                
            }

        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var storeId = sheetHeader["storeId"].ToObject<int>();//调出仓库id;
            var unitId = sheetHeader["unitId"].ToObject<int>();//供货商id
            var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();

            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, -totalFee, flowSheet);

            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                var number = sheetItem["number"].ToObjectWithDefault<int>();//入库数量
                //清除采购记录
                var materialBuy = await MaterialBuyManager.GetAll()
                    .Where(o => o.UnitId == unitId && o.MaterialId == materialId && o.FlowSheetId == flowSheet.RelSheetId && o.BuyNumber == number)
                    .FirstOrDefaultAsync();
                if (materialBuy != null)
                {
                    await MaterialBuyManager.DeleteAsync(materialBuy);
                }
                await StoreMaterialManager.CountMaterial(storeId, materialId, -number, flowSheet);

            }
        }
    }
}

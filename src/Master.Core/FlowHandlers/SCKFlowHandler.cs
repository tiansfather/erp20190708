using Abp.UI;
using Master.Entity;
using Master.Extensions;
using Master.Storage;
using Master.Units;
using Master.WorkFlow;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowHandlers
{
    /// <summary>
    /// 票劵出库单
    /// </summary>
    public class SCKFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public MaterialSellManager MaterialSellManager { get; set; }
        public MaterialSellOutManager MaterialSellOutManager { get; set; }
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
            var storeId = sheetHeader["storeId"].ToObject<int>();//出货仓库id;
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理商id
            var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
            flowSheet.UnitId = unitId;
            flowSheet.SetPropertyValue("StoreName", sheetHeader["storeName"].ToObjectWithDefault<string>());
            flowSheet.SetPropertyValue("Fee", totalFee);

            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, -totalFee, flowSheet);
            var materialSellIds = new List<int>();
            var toOutMaterials = new Dictionary<int, int>();
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = sheetItem["materialId"].ToObjectWithDefault<int>();//商品Id
                var sellMaterialId = Convert.ToInt32(sheetItem["id"]);//对应的销售物料Id               
                var number = sheetItem["number"].ToObjectWithDefault<int>();//出货数量
                if (number == 0)
                {
                    continue;
                }
                //建立销售出库记录
                var materialSellOut = new MaterialSellOut()
                {
                    UnitId=unitId,
                    FlowSheetId=sheetItem["flowSheetId"].ToObjectWithDefault<int>(),
                    MaterialId=materialId,
                    OutNumber=number,
                    Price=sheetItem["price"].ToObjectWithDefault<decimal>(),
                    Discount=sheetItem["discount"].ToObjectWithDefault<decimal>()
                };
                await MaterialSellOutManager.InsertAsync(materialSellOut);

                materialSellIds.Add(sellMaterialId);
                //加入待出库集合
                if (!toOutMaterials.ContainsKey(materialId))
                {
                    toOutMaterials.Add(materialId, number);
                }
                else
                {
                    toOutMaterials[materialId] += number;
                }
                
                await MaterialSellManager.Out(unitId, sellMaterialId, storeId,number, flowSheet);
                
            }
            //出库
            foreach(var item in toOutMaterials)
            {
                await StoreMaterialManager.CountMaterial(storeId, item.Key, -item.Value, flowSheet);
            }
            await MaterialSellManager.CheckSellSheetStatus(materialSellIds);
        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var storeId = sheetHeader["storeId"].ToObject<int>();//出货仓库id;
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理商id
            var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, totalFee, flowSheet);
            var materialSellIds = new List<int>();
            var toOutMaterials = new Dictionary<int, int>();
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = sheetItem["materialId"].ToObjectWithDefault<int>();//商品Id
                var sellMaterialId = Convert.ToInt32(sheetItem["id"]);//对应的销售物料Id                
                var number = sheetItem["number"].ToObjectWithDefault<int>();//出货数量
                if (number == 0)
                {
                    continue;
                }
                materialSellIds.Add(sellMaterialId);
                //加入待出库集合
                if (!toOutMaterials.ContainsKey(materialId))
                {
                    toOutMaterials.Add(materialId, number);
                }
                else
                {
                    toOutMaterials[materialId] += number;
                }

                await MaterialSellManager.Out(unitId, sellMaterialId, storeId, -number, flowSheet);

            }
            //出库
            foreach (var item in toOutMaterials)
            {
                await StoreMaterialManager.CountMaterial(storeId, item.Key, item.Value, flowSheet);
            }
            await MaterialSellManager.CheckSellSheetStatus(materialSellIds);
        }
    }
}

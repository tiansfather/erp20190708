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
    /// 票劵退库单
    /// </summary>
    public class SRJFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public MaterialSellManager MaterialSellManager { get; set; }
        public MaterialSellBackManager MaterialSellBackManager { get; set; }
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
            var storeId = sheetHeader["storeId"].ToObject<int>();//退货仓库id;
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理商id
            var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
            var startDate = sheetHeader["startDate"].ToObjectWithDefault<DateTime>();
            flowSheet.UnitId = unitId;
            flowSheet.SetPropertyValue("StoreName", sheetHeader["storeName"].ToObjectWithDefault<string>());
            flowSheet.SetPropertyValue("Fee", totalFee);

            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, totalFee, flowSheet);

            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//退货数量
                await MaterialSellManager.Back(unitId, startDate, materialId, storeId,number, flowSheet);
                //产生退货数据
                var materialSellBack = new MaterialSellBack()
                {
                    UnitId = unitId,
                    MaterialId = materialId,
                    BackNumber = number,
                    FlowSheetId = flowSheet.Id,
                    Discount = sheetItem["discount"].ToObjectWithDefault<decimal>(),
                    Price = sheetItem["price"].ToObjectWithDefault<decimal>(),
                };
                await MaterialSellBackManager.InsertAsync(materialSellBack);
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
            var startDate = sheetHeader["startDate"].ToObjectWithDefault<DateTime>();
            //更改往来单位金额
            await UnitManager.ChangeFee(unitId, null, -totalFee, flowSheet);

            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                var number = sheetItem["number"].ToObjectWithDefault<int>();//退货数量

                await MaterialSellManager.Back(unitId, startDate, materialId, storeId, -number, flowSheet);

            }
        }
    }
}

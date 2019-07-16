using Abp.UI;
using Master.Entity;
using Master.Extensions;
using Master.Storage;
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
    /// 调拨单
    /// </summary>
    public class IALFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var outStoreId = sheetHeader["outStoreId"].ToObject<int>();//调出仓库id;
            var inStoreId = sheetHeader["inStoreId"].ToObject<int>();//调入仓库id;
            flowSheet.SetPropertyValue("OutStoreName", sheetHeader["outStoreName"].ToObjectWithDefault<string>());
            flowSheet.SetPropertyValue("InStoreName", sheetHeader["inStoreName"].ToObjectWithDefault<string>());
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//调拨数量

                await StoreMaterialManager.TransferMaterial(outStoreId,inStoreId, materialId, number,flowSheet);
                
            }

        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var outStoreId = sheetHeader["outStoreId"].ToObject<int>();//调出仓库id;
            var inStoreId = sheetHeader["inStoreId"].ToObject<int>();//调入仓库id;
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                var number = sheetItem["number"].ToObjectWithDefault<int>();//调拨数量

                await StoreMaterialManager.TransferMaterial(inStoreId, outStoreId, materialId, number, flowSheet);

            }
        }
    }
}

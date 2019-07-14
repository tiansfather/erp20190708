using Abp.UI;
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
    /// 装配单
    /// </summary>
    public class IASFlowHandler : FlowHandlerBase
    {
        public FlowSheetManager FlowSheetManager { get; set; }
        public FlowInstanceManager FlowInstanceManager { get; set; }
        public MaterialManager MaterialManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public override async Task Handle(FlowInstance instance, FlowForm flowForm)
        {
            var formData = instance.FormData;
            var formKey = flowForm.FormKey;
            //生成单据
            var flowSheet = new FlowSheet()
            {
                FlowInstanceId = instance.Id,
                SheetName = instance.InstanceName,
                FormKey = formKey,
            };
            var sheetId = await FlowSheetManager.InsertAndGetIdAsync(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(formData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//组合数量

                await StoreMaterialManager.CombineMaterial(storeId, materialId, number,flowSheet);
                
            }

        }

        public override async Task HandleRevert(FlowInstance flowInstance,FlowSheet flowSheet)
        {
            
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                var number = sheetItem["number"].ToObjectWithDefault<int>();//拆散数量

                await StoreMaterialManager.BreakMaterial(storeId, materialId, number, flowSheet);

            }
        }
    }
}

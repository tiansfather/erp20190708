using Abp.UI;
using Master.Entity;
using Master.Extensions;
using Master.Module;
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
    /// 报损报溢单
    /// </summary>
    public class ICHFlowHandler : FlowHandlerBase
    {
        public MaterialManager MaterialManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }

        public override async Task<FlowSheet> CreateSheet(FlowInstance instance, FlowForm flowForm)
        {
            var flowSheet=await base.CreateSheet(instance, flowForm);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            flowSheet.SetPropertyValue("StoreName", sheetHeader["storeName"].ToObjectWithDefault<string>());
            flowSheet.OrderStatus= "待审核";
            return flowSheet;
        }

        public override async Task Handle(FlowSheet flowSheet)
        {
            //await base.Handle(flowSheet);
            ////数据处理
            //var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            //var sheetData = formObj["sheetData"];
            //var sheetHeader = sheetData["header"];

            //var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            //foreach (var sheetItem in sheetData["body"])
            //{
            //    var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
            //    var number= sheetItem["number"].ToObjectWithDefault<int>();//报损报溢数量

            //    await StoreMaterialManager.CountMaterial(storeId, materialId, number, flowSheet);
                
            //}

        }

        public override async Task<IEnumerable<ModuleButton>> GetFlowBtns(FlowSheet flowSheet)
        {
            var btns = new List<ModuleButton>();
            if (flowSheet.OrderStatus == "待审核")
            //if (true)
            {
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "verify",
                    ButtonName = "审核",
                    ConfirmMsg = "本操作将对库存产生实际影响，确定继续吗？"
                });
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "cancel",
                    ButtonName = "取消",
                    ButtonClass="layui-btn-danger",
                    ConfirmMsg = "确认取消此单据？"
                });
            }
            return btns;
        }

        public override async Task Action(FlowSheet flowSheet, string action)
        {
            if (action == "verify")
            {
                var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
                var sheetData = formObj["sheetData"];
                var sheetHeader = sheetData["header"];

                var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
                foreach (var sheetItem in sheetData["body"])
                {
                    var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                    var number = sheetItem["number"].ToObjectWithDefault<int>();//报损报溢数量

                    await StoreMaterialManager.CountMaterial(storeId, materialId, number, flowSheet);

                }
                //设置为审核状态
                flowSheet.OrderStatus = "已审核";
            }else if (action == "cancel")
            {
                flowSheet.OrderStatus = "已取消";
            }
        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {

            //数据处理
            //var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            //var sheetData = formObj["sheetData"];
            //var sheetHeader = sheetData["header"];

            //var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            //foreach (var sheetItem in sheetData["body"])
            //{
            //    var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

            //    var number = sheetItem["number"].ToObjectWithDefault<int>();//数量

            //    await StoreMaterialManager.CountMaterial(storeId, materialId, -number, flowSheet);

            //}
        }
    }
}

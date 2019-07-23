﻿using Abp.UI;
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

            return flowSheet;
        }

        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//报损报溢数量

                await StoreMaterialManager.CountMaterial(storeId, materialId, number, flowSheet);
                
            }

        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {

            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            var storeId = sheetHeader["storeId"].ToObject<int>();//仓库id;
            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id

                var number = sheetItem["number"].ToObjectWithDefault<int>();//数量

                await StoreMaterialManager.CountMaterial(storeId, materialId, -number, flowSheet);

            }
        }
    }
}
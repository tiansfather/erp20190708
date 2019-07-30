using Abp.UI;
using Master.Entity;
using Master.Extensions;
using Master.Finance;
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
    /// 账户调拨单
    /// </summary>
    public class CRRFlowHandler : FlowHandlerBase
    {
        public FeeAccountManager FeeAccountManager { get; set; }
        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            flowSheet.SheetDate= sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            var inAccountId= sheetHeader["inAccountId"].ToObjectWithDefault<int>();//账号id;
            var outAccountId= sheetHeader["outAccountId"].ToObject<int>();
            var fee= sheetHeader["fee"].ToObject<decimal>();//
            var inAccount = await FeeAccountManager.GetByIdAsync(inAccountId);
            var outAccount = await FeeAccountManager.GetByIdAsync(outAccountId);
            flowSheet.SetPropertyValue("Fee", fee);
            flowSheet.SetPropertyValue("OutAccountId", outAccountId);
            flowSheet.SetPropertyValue("InAccountId", inAccountId);
            flowSheet.SetPropertyValue("OutAccount", outAccount.Name);
            flowSheet.SetPropertyValue("InAccount", inAccount.Name);

            //账户金额变动
            await FeeAccountManager.BuildFeeHistory(inAccount,null, fee, flowSheet);
            await FeeAccountManager.BuildFeeHistory(outAccount,null, -fee, flowSheet);
        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            var inAccountId = flowSheet.GetPropertyValue<int>("InAccountId");
            var outAccountId = flowSheet.GetPropertyValue<int>("OutAccountId");
            var inAccount = await FeeAccountManager.GetByIdAsync(inAccountId);
            var outAccount = await FeeAccountManager.GetByIdAsync(outAccountId);
            var fee = flowSheet.GetPropertyValue<decimal>("Fee");
            //账户金额变动
            await FeeAccountManager.BuildFeeHistory(inAccount,null, -fee, flowSheet);
            await FeeAccountManager.BuildFeeHistory(outAccount,null, fee, flowSheet);
        }
    }
}

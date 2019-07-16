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
    /// 销售收款单
    /// </summary>
    public class SSKFlowHandler : FlowHandlerBase
    {
        public UnitManager UnitManager { get; set; }
        public FeeAccountManager FeeAccountManager { get; set; }
        private string GetPayTypeName(int payType)
        {
            switch (payType)
            {
                case 0:
                    return "现金收款";
                case 1:
                    return "转账收款";
                case 2:
                    return "支票收款";
                default:
                    return "";
            }
        }
        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理id;
            var accountId= sheetHeader["accountId"].ToObjectWithDefault<int?>();//账号id;
            var payType= sheetHeader["payType"].ToObject<int>();
            var totalFee= sheetHeader["totalFee"].ToObject<decimal>();//
            flowSheet.UnitId = unitId;
            flowSheet.SetPropertyValue("Fee", totalFee);
            flowSheet.SetPropertyValue("PayType", GetPayTypeName(payType));            
            //读取对应的账号id
            if (payType == 0)
            {
                accountId = (await FeeAccountManager.GetByName(FeeAccount.StaticAccountName1)).Id;
            }else if (payType == 2)
            {
                accountId = (await FeeAccountManager.GetByName(FeeAccount.StaticAccountName2)).Id;
            }
            flowSheet.SetPropertyValue("AccountId", accountId);
            //往来单位金额变动
            await UnitManager.ChangeFee(unitId, accountId.Value, totalFee, flowSheet);

            foreach (var sheetItem in sheetData["body"])
            {
                
                
            }

        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            var accountId = flowSheet.GetPropertyValue<int>("AccountId");
            var totalFee = flowSheet.GetPropertyValue<decimal>("Fee");
            //往来单位金额变动
            await UnitManager.ChangeFee(flowSheet.UnitId.Value, accountId, -totalFee, flowSheet);
        }
    }
}

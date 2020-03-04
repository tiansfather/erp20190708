﻿using Abp.UI;
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
    /// 采购付款单
    /// </summary>
    public class PPYFlowHandler : FlowHandlerBase
    {
        public UnitManager UnitManager { get; set; }
        public FeeAccountManager FeeAccountManager { get; set; }
        public FeeCheckManager FeeCheckManager { get; set; }
        private string GetPayTypeName(int payType)
        {
            switch (payType)
            {
                case 0:
                    return "现金付款";
                case 1:
                    return "转账付款";
                case 2:
                    return "支票付款";
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
            flowSheet.SetPropertyValue("RelCompanyName", sheetHeader["relCompanyName"]?.ToObject<string>());
            //读取对应的账号id
            if (payType == 0)
            {
                accountId = (await FeeAccountManager.GetByName(FeeAccount.StaticAccountName1)).Id;
            }else if (payType == 2)
            {
                accountId = (await FeeAccountManager.GetByName(FeeAccount.StaticAccountName2)).Id;
                //建立支票信息
                var feeCheckId = sheetHeader["feeCheck"]["id"].ToObject<int>();
                //设置对应支票信息为已支出
                var feeCheck = await FeeCheckManager.GetByIdAsync(feeCheckId);
                if (feeCheck.CheckStatus != CheckStatus.收入)
                {
                    throw new UserFriendlyException("支票状态不是收入，无法进行使用");
                }
                feeCheck.CheckStatus = CheckStatus.支出;
                feeCheck.OutFlowSheetId = flowSheet.Id;//设置支票的支出单据
                flowSheet.SetPropertyValue("FeeCheckId", feeCheckId);
            }
            flowSheet.SetPropertyValue("AccountId", accountId);
            //往来单位金额变动
            await UnitManager.ChangeFee(unitId, accountId.Value, -totalFee, flowSheet);

            

        }

        public override async Task HandleRevert(FlowSheet flowSheet)        {
            var accountId = flowSheet.GetPropertyValue<int>("AccountId");
            var totalFee = flowSheet.GetPropertyValue<decimal>("Fee");
            //往来单位金额变动
            await UnitManager.ChangeFee(flowSheet.UnitId.Value, accountId, +totalFee, flowSheet);
            //将对应的支票设置为收入退回
            if(flowSheet.GetPropertyValue<string>("PayType")== GetPayTypeName(2))
            {
                var feeCheckId = flowSheet.GetPropertyValue<int>("FeeCheckId");
                var feeCheck = await FeeCheckManager.GetByIdAsync(feeCheckId);
                feeCheck.CheckStatus = CheckStatus.支出退票;
            }
        }
    }
}

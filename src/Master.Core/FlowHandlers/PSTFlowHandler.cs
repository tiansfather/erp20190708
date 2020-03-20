using Master.Entity;
using Master.Extensions;
using Master.Finance;
using Master.Module;
using Master.Units;
using Master.WorkFlow;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowHandlers
{
    public class PSTFlowHandler: FlowHandlerBase
    {
        public UnitManager UnitManager { get; set; }
        public FeeAccountManager FeeAccountManager { get; set; }
        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);

            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var payUnitId = sheetHeader["payUnit"]["id"].ToObject<int>();//付款代理id
            var payUnit = await UnitManager.GetByIdAsync(payUnitId);            
            var receiveUnitId = sheetHeader["receiveUnit"]["id"].ToObject<int>();//收款供应商id
            var receiveUnit = await UnitManager.GetByIdAsync(receiveUnitId);
            var fee = sheetHeader["fee"].ToObject<decimal>();//
            flowSheet.SetPropertyValue("Fee", fee);
            flowSheet.SetPropertyValue("OutUnitName", payUnit.UnitName);
            flowSheet.SetPropertyValue("OutCompanyName", sheetHeader["payUnit"]["companyName"].ToObjectWithDefault<string>());
            flowSheet.SetPropertyValue("InUnitName", receiveUnit.UnitName);
            flowSheet.SetPropertyValue("InCompanyName", sheetHeader["receiveUnit"]["companyName"].ToObjectWithDefault<string>());
            //读取对应的账号id
            var accountId = (await FeeAccountManager.GetByName(FeeAccount.StaticAccountName3)).Id;            
            flowSheet.SetPropertyValue("AccountId", accountId);
            //往来单位金额变动
            await UnitManager.ChangeFee(payUnitId, accountId, fee, flowSheet);
            await UnitManager.ChangeFee(receiveUnitId, accountId,- fee, flowSheet);
        }
        public override async Task<IEnumerable<ModuleButton>> GetFlowBtns(FlowSheet flowSheet)
        {
            var result = new List<ModuleButton>();
            //加入打印按钮
            result.Add(new ModuleButton()
            {
                ButtonKey = "print",
                ButtonName = "打印",
                ButtonClass = "layui-btn-normal"
            });
            return result;
        }

        public override Task HandleRevert(FlowSheet flowSheet)
        {
            throw new NotImplementedException();
        }
    }
}

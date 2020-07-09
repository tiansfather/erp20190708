using Abp.Domain.Entities;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class InvoiceBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var btns = base.GetModuleButtons();
            btns.Add(new ModuleButton()
            {
                ButtonKey = "Edit",
                ButtonName = "修改",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionUrl = "/Invoice/Edit",
                Sort = 1
            });
            btns.Add(new ModuleButton()
            {
                ButtonKey = "Verify",
                ButtonName = "审核",
                ButtonActionType = ButtonActionType.Func,
                ButtonType = ButtonType.ForSingleRow,
                ConfirmMsg="确认审核这些发票？",
                //ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
                //ButtonActionUrl = "abp.services.app.invoice.verify",
                ButtonActionUrl= "doVerify({{d.id}},{{d.unitId}},numBack('{{d.fee}}'))",
                Sort=1
            });
            btns.Add(new ModuleButton()
            {
                ButtonKey = "Close",
                ButtonName = "关闭",
                ButtonActionType = ButtonActionType.Ajax,
                ButtonType = ButtonType.ForSelectedRows,
                ConfirmMsg = "确认关闭这些发票？",
                //ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
                ButtonActionUrl = "abp.services.app.invoice.close",
                Sort=2,
                ButtonClass="layui-btn-danger"
            });
            return btns;
        }
    }
}

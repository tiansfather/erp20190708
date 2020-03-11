using Master.Module;
using System;
using System.Collections.Generic;
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
                ButtonKey = "Verify",
                ButtonName = "审核",
                ButtonActionType = ButtonActionType.Ajax,
                ButtonType = ButtonType.ForSelectedRows,
                //ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
                ButtonActionUrl = "abp.services.app.invoice.verify"
            });
            btns.Add(new ModuleButton()
            {
                ButtonKey = "Close",
                ButtonName = "关闭",
                ButtonActionType = ButtonActionType.Ajax,
                ButtonType = ButtonType.ForSelectedRows,
                //ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
                ButtonActionUrl = "abp.services.app.invoice.close"
            });
            return btns;
        }
    }
}

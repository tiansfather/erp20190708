using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class FeeAccountBaseModules : BaseSystemModules
    {
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var editBtn = ButtonInfos.Where(o => o.ButtonKey == "Edit").Single();
            editBtn.ClientShowCondition = "d.name!='现金账户' && d.name!='支票账户' && d.name!='过账账户'";
            var delBtn= ButtonInfos.Where(o => o.ButtonKey == "Delete").Single();
            delBtn.ClientShowCondition = "d.name!='现金账户' && d.name!='支票账户' && d.name!='过账账户'";
            var multiEditBtn = ButtonInfos.Where(o => o.ButtonKey == "MultiEdit").Single();
            ButtonInfos.Remove(multiEditBtn);
        }
    }
}

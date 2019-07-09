using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class StoreBaseModules : BaseSystemModules
    {
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var delBtn = ButtonInfos.Where(o => o.ButtonKey == "Delete").Single();
            ButtonInfos.Remove(delBtn);
        }
    }
}

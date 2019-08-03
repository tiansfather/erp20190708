using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class MaterialBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons = base.GetModuleButtons();

            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Import",
                ButtonName = "导入",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForNoneRow,
                IsEnabled = true,
                ButtonActionUrl = "/Import/?type=Master.Storage.MaterialImportDto",
                ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[]}",
                Sort = 0
            });

            return buttons;
        }
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            ColumnInfos.Remove(ColumnInfos.Single(o => o.ColumnKey == "CreatorUserId"));
            ColumnInfos.Remove(ColumnInfos.Single(o => o.ColumnKey == "CreationTime"));
            ColumnInfos.Remove(ColumnInfos.Single(o => o.ColumnKey == "LastModifierUserId"));
            ColumnInfos.Remove(ColumnInfos.Single(o => o.ColumnKey == "LastModificationTime"));
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var delBtn = ButtonInfos.Where(o => o.ButtonKey == "Delete").Single();
            ButtonInfos.Remove(delBtn);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class UnitBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            return new List<ColumnInfo>();
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var btns = new List<ModuleButton>();
            //btns.Add(new ModuleButton()
            //{
            //    ButtonKey = "Import",
            //    ButtonName = "导入",
            //    ButtonActionType = ButtonActionType.Form,
            //    ButtonType = ButtonType.ForNoneRow,
            //    IsEnabled = true,
            //    ButtonActionUrl = "/Import/?type=Master.Units.UnitImportDto",
            //    ButtonActionParam = "{\"area\": [\"100%\", \"100%\"],\"btn\":[]}",
            //    Sort = 5
            //});
            ////增加查看客户查看供应商权限
            //var ViewCustomerButton = new ModuleButton()
            //{
            //    ButtonKey = "ViewCustomer",
            //    ButtonName = "查看客户",
            //    ButtonActionType = ButtonActionType.Resource,
            //    IsEnabled=true,
            //    RequirePermission=false
            //};
            //btns.Add(ViewCustomerButton);
            //var ViewSupplierButton = new ModuleButton()
            //{
            //    ButtonKey = "ViewSupplier",
            //    ButtonName = "查看供应商",
            //    ButtonActionType = ButtonActionType.Resource,
            //    IsEnabled = true,
            //    RequirePermission = false
            //};
            //btns.Add(ViewSupplierButton);
            return btns;
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
    }
}

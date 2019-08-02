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
            //增加账号按钮
            var AccountNumberModuleButton = new ModuleButton()
            {
                ButtonKey = "Account",
                ButtonName = "账号",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/Home/Show?name=../Unit/Account",
                ButtonActionParam = "{\"area\": [\"60%\", \"60%\"]}",
                ButtonClass = "",
                Sort = 6
            };
            btns.Add(AccountNumberModuleButton);
            btns.Add(new ModuleButton()
            {
                ButtonKey = "Invoice",
                ButtonName = "开票资料",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                IsEnabled = true,
                ButtonActionUrl = "/Unit/Invoice",
                ButtonActionParam = "{\"area\": [\"60%\", \"60%\"]}",
                Sort = 5
            });
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

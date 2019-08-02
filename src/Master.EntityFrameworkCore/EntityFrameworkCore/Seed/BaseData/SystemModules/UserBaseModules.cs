
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class UserBaseModules:BaseSystemModules
    {
        /// <summary>
        /// 得到对应的基础附加列
        /// </summary>
        /// <returns></returns>
        public override List<ColumnInfo> GetColumnInfos()
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>();

            //增加员工部门列
            //var departColumnInfo = new ColumnInfo()
            //{
            //    ColumnKey = "StaffOrganizationUnit",
            //    ColumnName = "部门",
            //    ColumnType = ColumnTypes.Customize,
            //    IsInterColumn = true,
            //    CustomizeControl = "IStaffDepartControl",
            //    ControlParameter = "{\"selectedMulti\":true}",
            //    Templet = "{{d.StaffOrganizationUnit_data}}"
            //};
            //columnInfos.Add(departColumnInfo);



            //增加角色列
            columnInfos.Add(new ColumnInfo()
            {
                ColumnKey = "RoleNames",
                ColumnName = "角色",
                ColumnType = ColumnTypes.Reference,
                RelativeDataType=RelativeDataType.Default,
                IsInterColumn = true,
                IsShownInAdd=false,
                IsShownInEdit=false,
                IsShownInAdvanceSearch=false,
                IsShownInMultiEdit=false
            });
            return columnInfos;
        }
        /// <summary>
        /// 补齐列的属性
        /// </summary>
        /// <returns></returns>
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
        /// <summary>
        /// 得到对应的基础附加按钮
        /// </summary>
        /// <returns></returns>
        public override List<ModuleButton> GetModuleButtons()
        {
            List<ModuleButton> moduleButtons = new List<ModuleButton>();
            
            //增加权限按钮(有账号的显示)
            var PermissionModuleButton = new ModuleButton()
            {
                ButtonKey = "Permission",
                ButtonName = "权限",
                ButtonType = ButtonType.ForSingleRow ,
                ButtonActionParam= "{\"btn\": []}",
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/Permission/Assign",
                ButtonClass = "",
                Sort = 6
            };
            moduleButtons.Add(PermissionModuleButton);

            //增加账号按钮
            var AccountNumberModuleButton = new ModuleButton()
            {
                ButtonKey = "Account",
                ButtonName = "账号",
                ButtonType = ButtonType.ForSingleRow ,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/User/Account",
                ButtonActionParam= "{\"area\": [\"100%\", \"100%\"]}",
                ButtonClass = "",
                Sort = 6
            };
            moduleButtons.Add(AccountNumberModuleButton);

            //增加离职按钮（在没有离职的时候显示）
            var DimissionModuleButton = new ModuleButton()
            {
                ButtonKey = "Dimission",
                ButtonName = "离职",
                ButtonType = ButtonType.ForSingleRow|ButtonType.ForSelectedRows,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionUrl = $"/User/OffJob",
                ButtonClass = "",
                ClientShowCondition = "!d.jobDateEnd",
                Sort = 10
            };
            moduleButtons.Add(DimissionModuleButton);
            return moduleButtons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            
        }
    }
}

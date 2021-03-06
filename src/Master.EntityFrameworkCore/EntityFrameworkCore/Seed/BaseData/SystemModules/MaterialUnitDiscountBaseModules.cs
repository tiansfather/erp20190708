﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData.SystemModules
{
    public class MaterialUnitDiscountBaseModules : BaseSystemModules
    {
        public override List<ModuleButton> GetModuleButtons()
        {
            var btns= base.GetModuleButtons();
            //btns.Add(new ModuleButton()
            //{
            //    ButtonKey = "View",
            //    ButtonName = "查看",
            //    ButtonActionType = ButtonActionType.Form,
            //    ButtonType = ButtonType.ForSingleRow,
            //    //ButtonActionParam = "{\"area\": [\"80%\", \"90%\"],\"btn\":null}",
            //    ButtonActionParam = "{\"btn\":null}",
            //    ButtonActionUrl = "/MaterialDIY/View",
            //    ButtonClass = "layui-btn-normal"
            //});
            return btns;
        }
        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "Add"));
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "MultiEdit"));
            ButtonInfos.Remove(ButtonInfos.Single(o => o.ButtonKey == "Delete"));

            var editBtn = ButtonInfos.Single(o => o.ButtonKey == "Edit");
            editBtn.ButtonName = "代理商折扣与销售方式";
        }
    }
}

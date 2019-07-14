using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class FlowFormBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            return new List<ColumnInfo>();
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var buttons = new List<ModuleButton>();
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Design",
                ButtonName = "表单设计",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                IsEnabled = true,
                ButtonActionUrl = "/FlowForm/Design",
                ButtonActionParam = "{\"area\": [\"100%\", \"100%\"]}",
                Sort = 3
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Handler",
                ButtonName = "处理程序",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                IsEnabled = true,
                ButtonActionUrl = "/FlowForm/Handler",
                ButtonActionParam = "{\"area\": [\"100%\", \"100%\"]}",
                Sort = 4
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "View",
                ButtonName = "预览",
                ButtonActionType = ButtonActionType.Form,
                ButtonType = ButtonType.ForSingleRow,
                IsEnabled = true,
                ButtonActionUrl = "/FlowForm/Input",
                ButtonActionParam = "{\"btn\": []}",
                Sort = 5
            });
            buttons.Add(new ModuleButton()
            {
                ButtonKey = "Init",
                ButtonName = "初始化",
                ButtonActionType = ButtonActionType.Ajax,
                ButtonType = ButtonType.ForNoneRow,
                IsEnabled = true,
                ButtonClass="layui-btn-normal",
                ButtonActionUrl = "abp.services.workflow.flowForm.initDefaultForm",
                ConfirmMsg="确认初始化?此操作不会删除现有表单",
                Sort = 0
            });
            return buttons;
        }

        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var multiEditBtn = ButtonInfos.Single(o => o.ButtonKey == "MultiEdit");
            ButtonInfos.Remove(multiEditBtn);
            var delBtn = ButtonInfos.Single(o => o.ButtonKey == "Delete");
            delBtn.ClientShowCondition = "!d.isSystemForm";
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
    }
}

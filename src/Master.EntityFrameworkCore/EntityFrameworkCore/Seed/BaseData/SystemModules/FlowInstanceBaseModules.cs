using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Master.Module;

namespace Master.EntityFrameworkCore.Seed.BaseData
{
    public class FlowInstanceBaseModules : BaseSystemModules
    {
        public override List<ColumnInfo> GetColumnInfos()
        {
            return new List<ColumnInfo>();
        }

        public override List<ModuleButton> GetModuleButtons()
        {
            var btns= new List<ModuleButton>();
            //添加详情按钮
            var ViewButton = new ModuleButton()
            {
                ButtonKey = "View",
                ButtonName = "查看",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"80%\"],\"btn\":null}",
                ButtonActionUrl = $"/FlowInstance/View",
                ButtonClass = "",
                Sort = 0
            };
            btns.Add(ViewButton);
            //添加处理按钮
            var DealButton = new ModuleButton()
            {
                ButtonKey = "Deal",
                ButtonName = "处理",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"80%\"]}",
                ButtonActionUrl = $"/FlowInstance/Deal",
                ButtonClass = "",
                ClientShowCondition="isToDealTab",
                Sort = 0
            };
            btns.Add(DealButton);
            //添加重发按钮
            var RePostButton = new ModuleButton()
            {
                ButtonKey = "RePost",
                ButtonName = "重发",
                ButtonType = ButtonType.ForSingleRow,
                ButtonActionType = ButtonActionType.Form,
                ButtonActionParam = "{\"area\": [\"80%\", \"80%\"]}",
                ButtonActionUrl = $"/FlowForm/InstanceRepost",
                ButtonClass = "",
                ClientShowCondition= "d.instanceStatus==4",
                Sort = 0
            };
            btns.Add(RePostButton);
            return btns;
        }
        public override void SetButtonsInfosMoreData(ICollection<ModuleButton> ButtonInfos)
        {
            var editButton = ButtonInfos.Where(o => o.ButtonKey == "Edit").Single();
            ButtonInfos.Remove(editButton);//移除编辑按钮
            var multiEditButton = ButtonInfos.Where(o => o.ButtonKey == "MultiEdit").Single();
            ButtonInfos.Remove(multiEditButton);//移除批量修改按钮
            var addButton = ButtonInfos.Where(o => o.ButtonKey == "Add").Single();
            addButton.ButtonName = "新的申请";
            
        }

        public override void SetColumnInfosMoreData(ICollection<ColumnInfo> ColumnInfos)
        {
            
        }
    }
}

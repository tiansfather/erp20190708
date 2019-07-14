using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("装配单查询", BaseType = typeof(FlowSheet),GenerateDefaultButtons =false,GenerateDefaultColumns =false)]
    public class IASSheet:FlowSheet
    {
        [InterColumn(ColumnName = "装配单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "交货日期", Sort = 10)]
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
    }
}

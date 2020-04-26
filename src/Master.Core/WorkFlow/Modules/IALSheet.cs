using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("调拨单查询", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class IALSheet: FlowSheet
    {
        [InterColumn(ColumnName = "调拨单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "发生日期", Sort = 2)]
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [NotMapped]
        [InterColumn(ColumnName ="调出仓库",ValuePath ="Property",Sort =21, EnableDataFilter = true)]
        public string OutStoreName { get; set; }
        [NotMapped]
        [InterColumn(ColumnName = "调入仓库", ValuePath = "Property", Sort = 22, EnableDataFilter = true)]
        public string InStoreName { get; set; }
    }
}

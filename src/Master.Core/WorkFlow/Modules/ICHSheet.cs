﻿using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("报损报溢查询", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class ICHSheet : FlowSheet
    {
        [InterColumn(ColumnName = "报损报溢单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "发生日期", ColumnType = Module.ColumnTypes.DateTime, Sort = 2)]
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [NotMapped]
        [InterColumn(ColumnName = "仓库", ValuePath = "Property", Sort = 21, EnableDataFilter = true)]
        public string StoreName { get; set; }
        [InterColumn(ColumnName = "当前状态", Sort = 22, EnableDataFilter = true, Templet = "{{getStatusHtml(d.orderStatus)}}")]
        public override string OrderStatus { get; set; }
        public override SheetNature SheetNature { get => base.SheetNature; set => base.SheetNature = value; }
    }
}

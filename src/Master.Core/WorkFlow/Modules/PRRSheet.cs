﻿using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("退货单查询", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class PRRSheet: FlowSheet
    {
        [InterColumn(ColumnName = "退货单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "供货商", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 2, EnableDataFilter = true)]
        public override int? UnitId { get => base.UnitId; set => base.UnitId = value; }
        [InterColumn(ColumnName = "退货时间", ColumnType = Module.ColumnTypes.DateTime, Sort = 3)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display||'/'}}", Sort = 4, EnableDataFilter = true)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName = "应收总额", ValuePath = "Property", ColumnType = Module.ColumnTypes.Number, Sort = 5, DisplayFormat = "N2")]
        [NotMapped]
        public virtual decimal Fee { get; set; }
        [InterColumn(ColumnName = "仓库", ValuePath = "Property", Sort = 6, EnableDataFilter = true)]
        [NotMapped]
        public virtual string StoreName { get; set; }
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }
    }
}

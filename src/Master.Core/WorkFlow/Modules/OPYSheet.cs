using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("其它付款单", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class OPYSheet : FlowSheet
    {
        [InterColumn(ColumnName = "付款单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "收款供应商", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 2)]
        public override int? UnitId { get => base.UnitId; set => base.UnitId = value; }
        [InterColumn(ColumnName = "制单时间",Sort =3)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display}}", Sort = 4)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName ="付款金额",ValuePath ="Property",ColumnType =Module.ColumnTypes.Number,Sort =5)]
        [NotMapped]
        public virtual decimal Fee{get;set;}
        [InterColumn(ColumnName = "付款类型", ValuePath = "Property",Sort =6)]
        [NotMapped]
        public virtual string PayType { get; set; }
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }
    }
}

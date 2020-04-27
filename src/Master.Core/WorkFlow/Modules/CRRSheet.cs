using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("账户调拨单", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class CRRSheet : FlowSheet
    {
        [InterColumn(ColumnName = "调拨单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "发生金额", ValuePath = "Property", ColumnType = Module.ColumnTypes.Number, Sort = 2)]
        [NotMapped]
        public virtual decimal Fee { get; set; }
        [InterColumn(ColumnName = "调出账号", ValuePath = "Property",Sort =3,EnableDataFilter =true)]
        [NotMapped]
        public virtual string OutAccount { get; set; }
        [InterColumn(ColumnName = "调入账号", ValuePath = "Property", Sort = 4, EnableDataFilter = true)]
        [NotMapped]
        public virtual string InAccount { get; set; }
        [InterColumn(ColumnName = "发生日期", Sort = 5,ColumnType =Module.ColumnTypes.DateTime,DisplayFormat ="yyyy-MM-dd")]
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [InterColumn(ColumnName = "制单时间",Sort =6)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display||'/'}}", Sort = 7, EnableDataFilter = true)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }

    }
}

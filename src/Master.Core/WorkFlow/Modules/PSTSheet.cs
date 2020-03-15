using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("过账单", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false, GenerateDefaultColumns = false)]
    public class PSTSheet:FlowSheet
    {
        [InterColumn(ColumnName = "过账单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "付款代理商", ValuePath = "Property", Sort = 2)]
        [NotMapped]
        public virtual string OutUnitName { get; set; }
        [InterColumn(ColumnName = "付款单位名称", ValuePath = "Property", Sort = 3)]
        [NotMapped]
        public virtual string OutCompanyName { get; set; }
        [InterColumn(ColumnName = "发生金额", ValuePath = "Property", ColumnType = Module.ColumnTypes.Number, Sort = 4)]
        [NotMapped]
        public virtual decimal Fee { get; set; }

        [InterColumn(ColumnName = "发生日期", Sort = 5, ColumnType = Module.ColumnTypes.DateTime, DisplayFormat = "yyyy-MM-dd")]
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        [InterColumn(ColumnName = "收款供应商", ValuePath = "Property", Sort = 6)]
        [NotMapped]
        public virtual string InUnitName { get; set; }
        [InterColumn(ColumnName = "收款单位名称", ValuePath = "Property", Sort = 7)]
        [NotMapped]
        public virtual string InCompanyName { get; set; }
        
        [InterColumn(ColumnName = "制单时间", Sort = 6)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display}}", Sort = 7)]
        public override long? CreatorUserId { get; set; }
    }
}

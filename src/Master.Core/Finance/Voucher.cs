using Master.Entity;
using Master.Module.Attributes;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Finance
{
    [InterModule("凭证查询",  GenerateDefaultButtons = false)]
    public class Voucher: BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName = "提交代理商", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 1)]
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        [InterColumn(ColumnName = "发生日期", Sort = 2, ColumnType = Module.ColumnTypes.DateTime, DisplayFormat = "yyyy-MM-dd HH:mm")]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display}}", Sort = 3)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName = "发生金额",  ColumnType = Module.ColumnTypes.Number, Sort = 4, Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"凭证查看\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/Home/Show?name=../Voucher/View\" onclick=\"func.callModuleButtonEvent()\">{{d.fee}}</a>")]
        [Column(TypeName = "decimal(20,2)")]
        public virtual decimal Fee { get; set; }
        [InterColumn(ColumnName = "状态", ColumnType = Module.ColumnTypes.Select ,DictionaryName ="Master.Finance.VoucherStatus",Templet = "{{d.voucherStatus_display}}",Sort =5)]
        public virtual VoucherStatus VoucherStatus { get; set; }
        [InterColumn(ColumnName = "关联单据", Sort =6)]
        public string RelSheetSN { get; set; }
        [InterColumn(ColumnName = "备注", Sort = 7)]
        public override string Remarks { get => base.Remarks; set => base.Remarks = value; }
        [NotMapped]
        public string FilePath
        {
            get
            {
                return this.GetPropertyValue<string>("FilePath");
            }
            set
            {
                this.SetPropertyValue("FilePath", value);
            }
        }
        [NotMapped]
        public string FileName
        {
            get
            {
                return this.GetPropertyValue<string>("FileName");
            }
            set
            {
                this.SetPropertyValue("FileName", value);
            }
        }
    }

    public enum VoucherStatus
    {
        待确认,
        已确认,
        已关闭
    }
}

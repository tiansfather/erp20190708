using Master.Entity;
using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Finance
{
    [InterModule("支票查询",  GenerateDefaultButtons = false, GenerateDefaultColumns = false)]
    public class FeeCheck:BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName ="支票编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"支票详情\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FeeCheck/View?moduleKey=FeeCheck\" onclick=\"func.callModuleButtonEvent()\">{{d.checkNumber}}</a>",Sort =1)]
        public string CheckNumber { get; set; }
        [InterColumn(ColumnName = "金额",ColumnType =Module.ColumnTypes.Number, Sort = 2)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal CheckFee { get; set; }
        [InterColumn(ColumnName = "开票日期", ColumnType = Module.ColumnTypes.DateTime, Sort = 3)]
        public DateTime CheckDate { get; set; }
        [InterColumn(ColumnName ="兑换期限", Sort = 4)]
        public int CheckDaySpan { get; set; }
        [InterColumn(ColumnName = "签发公司", Sort = 5)]
        public string CheckCompany { get; set; }
        [InterColumn(ColumnName = "银行", Sort = 6)]
        public string CheckBank { get; set; }
        [InterColumn(ColumnName = "状态",ColumnType =Module.ColumnTypes.Select,DictionaryName ="Master.Finance.CheckStatus",Templet = "{{d.checkStatus_display}}", Sort = 7)]
        public CheckStatus CheckStatus { get; set; }
        [InterColumn(ColumnName ="备注", Sort = 8)]
        public override string Remarks { get => base.Remarks; set => base.Remarks = value; }

        public int? InFlowSheetId { get; set; }
        public virtual FlowSheet InFlowSheet { get; set; }
        public int? OutFlowSheetId { get; set; }
        public virtual FlowSheet OutFlowSheet { get; set; }
    }

    public enum CheckStatus
    {
        收入,
        支出,
        收入退票,
        支出退票
    }
}

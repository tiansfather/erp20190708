using Master.Entity;
using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    [InterModule("支票查询",  GenerateDefaultButtons = false, GenerateDefaultColumns = false)]
    public class FeeCheck:BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName ="支票编号")]
        public string CheckNumber { get; set; }
        [InterColumn(ColumnName = "金额",ColumnType =Module.ColumnTypes.Number)]
        public decimal CheckFee { get; set; }
        [InterColumn(ColumnName = "开票日期", ColumnType = Module.ColumnTypes.DateTime)]
        public DateTime CheckDate { get; set; }
        [InterColumn(ColumnName ="兑换期限")]
        public int CheckDaySpan { get; set; }
        [InterColumn(ColumnName = "签发公司")]
        public string CheckCompany { get; set; }
        [InterColumn(ColumnName = "银行")]
        public string CheckBank { get; set; }
        [InterColumn(ColumnName = "状态",ColumnType =Module.ColumnTypes.Select,DictionaryName ="Master.Finance.CheckStatus",Templet = "{{d.checkStatus_display}}")]
        public CheckStatus CheckStatus { get; set; }
        [InterColumn(ColumnName ="备注")]
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

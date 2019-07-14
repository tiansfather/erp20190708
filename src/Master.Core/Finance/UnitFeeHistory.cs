using Master.Entity;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    /// <summary>
    /// 往来单位费用变动明细
    /// </summary>
    public class UnitFeeHistory : BaseFullEntityWithTenant
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// 变化的金额,正数为应付增加，负数为应付减少
        /// </summary>
        public decimal Fee { get; set; }
        public decimal RemainFee { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 关联单据
        /// </summary>
        public int? FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
    }
}

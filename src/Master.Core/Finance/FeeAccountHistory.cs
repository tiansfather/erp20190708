using Master.Entity;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    /// <summary>
    /// 账号金额变动明细
    /// </summary>
    public class FeeAccountHistory : BaseFullEntityWithTenant
    {
        public int FeeAccountId { get; set; }
        public virtual FeeAccount FeeAccount { get; set; }
        /// <summary>
        /// 变化的金额,正数为应付增加，负数为应付减少
        /// </summary>
        public decimal Fee { get; set; }
        public decimal RemainFee { get; set; }
        public string ChangeType { get; set; }
        /// <summary>
        /// 关联单据
        /// </summary>
        public int? FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
    }
}

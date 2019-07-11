using Master.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 工作流实例操作记录
    /// </summary>
    [Table("FlowInstanceOperationHistory")]
    public class FlowInstanceOperationHistory:BaseFullEntityWithTenant
    {
        public int FlowInstanceId { get; set; }
        public virtual FlowInstance FlowInstance { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        [Description("操作内容")]
        public string Content { get; set; }
    }
}

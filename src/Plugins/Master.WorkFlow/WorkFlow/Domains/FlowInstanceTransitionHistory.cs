using Master.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 工作流实例流转历史记录
    /// </summary>
    [Table("FlowInstanceTransitionHistory")]
    public class FlowInstanceTransitionHistory : BaseFullEntityWithTenant
    {
        public int FlowInstanceId { get; set; }
        public virtual FlowInstance FlowInstance { get; set; }

        /// <summary>
        /// 开始节点Id
        /// </summary>
        [Description("开始节点Id")]
        public string FromNodeId { get; set; }
        /// <summary>
        /// 开始节点类型
        /// </summary>
        [Description("开始节点类型")]
        public int? FromNodeType { get; set; }
        /// <summary>
        /// 开始节点名称
        /// </summary>
        [Description("开始节点名称")]
        public string FromNodeName { get; set; }
        /// <summary>
        /// 结束节点Id
        /// </summary>
        [Description("结束节点Id")]
        public string ToNodeId { get; set; }
        /// <summary>
        /// 结束节点类型
        /// </summary>
        [Description("结束节点类型")]
        public int? ToNodeType { get; set; }
        /// <summary>
        /// 结束节点名称
        /// </summary>
        [Description("结束节点名称")]
        public string ToNodeName { get; set; }
        /// <summary>
        /// 转化状态
        /// </summary>
        [Description("转化状态")]
        public int TransitionSate { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        [Description("流程状态")]
        public InstanceStatus InstanceStatus { get; set; }
    }
}

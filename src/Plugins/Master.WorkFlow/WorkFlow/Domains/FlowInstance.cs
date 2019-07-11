using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 工作流流程实例表
    /// </summary>
    [Table("FlowInstance")]
    [InterModule("我的流程")]
    public class FlowInstance:BaseFullEntityWithTenant,IPassivable
    {
        /// <summary>
        /// 流程实例模板Id
        /// </summary>
        [Description("模板Id")]
        public int? FlowSchemeId { get; set; }
        public virtual FlowScheme FlowScheme { get; set; }
        /// <summary>
        /// 实例编号
        /// </summary>
        [Description("实例编号")]
        [InterColumn(ColumnName ="流程编号")]
        public string Code { get; set; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        [Description("流程名称")]
        [InterColumn(ColumnName = "流程名称")]
        public string InstanceName { get; set; }
        /// <summary>
        /// 当前节点ID
        /// </summary>
        [Description("当前节点ID")]
        public string ActivityId { get; set; }
        /// <summary>
        /// 当前节点类型（0会签节点）
        /// </summary>
        [Description("当前节点类型（0会签节点）")]
        public int? ActivityType { get; set; }
        /// <summary>
        /// 当前节点名称
        /// </summary>
        [Description("当前节点名称")]
        [InterColumn(ColumnName = "当前节点名称")]
        public string ActivityName { get; set; }
        /// <summary>
        /// 前一个ID
        /// </summary>
        [Description("前一个ID")]
        public string PreviousId { get; set; }
        /// <summary>
        /// 流程模板内容
        /// </summary>
        [Description("流程模板内容")]
        public string SchemeContent { get; set; }
        /// <summary>
        /// 表单内容
        /// </summary>
        [Description("表单内容")]
        public string FormContent { get; set; }
        /// <summary>
        /// 表单数据
        /// </summary>
        [Description("表单数据")]
        public string FormData { get; set; }
        
        /// <summary>
        /// 表单ID
        /// </summary>
        [Description("表单ID")]
        public int FlowFormId { get; set; }
        public FormType FormType { get; set; }
        public virtual FlowForm FlowForm { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [Description("等级")] 
        public int FlowLevel { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        [Description("流程状态")]
        [InterColumn(ColumnName = "流程状态",Templet = "{{# if(d.instanceStatus == 0){ }}<span class=\"layui-badge layui-bg-blue\">正在运行</span>{{# } else if(d.instanceStatus == 3){ }}<span class=\"layui-badge\">不同意</span>{{# } else if(d.instanceStatus == 4){ }}<span class=\"layui-badge layui-bg-gray\">被驳回</span>{{# } else{}}<span class=\"layui-badge layui-bg-green\">审批通过</span>{{# } }}")]
        public InstanceStatus InstanceStatus { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        [Description("执行人")]
        public string MakerList { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum InstanceStatus
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Processing=0,
        /// <summary>
        /// 已通过
        /// </summary>
        Finish=1,
        /// <summary>
        /// 不同意
        /// </summary>
        DisAgree=3,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject=4
    }
}

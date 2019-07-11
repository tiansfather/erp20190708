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
    /// 工作流模板信息表
    /// </summary>
    [Table("FlowScheme")]
    [InterModule("流程设计",SortField ="Sort",SortType =Module.SortType.Asc)]
    public class FlowScheme:BaseFullEntityWithTenant,IPassivable,IHaveSort
    {
        /// <summary>
        /// 流程编号
        /// </summary>
        [Description("流程编号")]
        [InterColumn(ColumnName ="流程编号")]
        public string SchemeCode { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        [Description("流程名称")]
        [InterColumn(ColumnName = "流程名称",VerifyRules ="required")]
        public string SchemeName { get; set; }
        /// <summary>
        /// 流程分类
        /// </summary>
        [Description("流程分类")]
        public string SchemeType { get; set; }
        /// <summary>
        /// 流程内容版本
        /// </summary>
        [Description("流程版本")]
        [InterColumn(ColumnName = "流程版本")]
        public string SchemeVersion { get; set; }
        /// <summary>
        /// 流程模板使用者
        /// </summary>
        [Description("流程模板使用者")]
        public string SchemeCanUser { get; set; }
        /// <summary>
        /// 流程内容
        /// </summary>
        [Description("流程内容")]
        public string SchemeContent { get; set; }
        /// <summary>
        /// 表单ID
        /// </summary>
        [Description("表单ID")]
        [InterColumn(ColumnName = "表单名称",ValuePath = "FlowForm.FormName")]
        public int FlowFormId { get; set; }
        public virtual FlowForm FlowForm { get; set; }
        
        /// <summary>
        /// 模板权限类型：0完全公开,1指定部门/人员
        /// </summary>
        [Description("模板权限类型：0完全公开,1指定部门/人员")]
        public int AuthorizeType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [InterColumn(ColumnName = "有效",ColumnType =Module.ColumnTypes.Switch)]
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}

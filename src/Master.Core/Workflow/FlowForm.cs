using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow
{
    /// <summary>
    /// 表单模板表
    /// </summary>
    [Table("FlowForm")]
    [InterModule("表单定义", SortField = "Sort", SortType = Module.SortType.Asc,GenerateOperateColumn =true)]
    public class FlowForm:BaseFullEntityWithTenant,IPassivable,IHaveSort
    {
        [InterColumn(ColumnName = "表单标志", VerifyRules = "required")]
        public string FormKey { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        [Description("表单名称")]
        [InterColumn(ColumnName ="表单名称",VerifyRules ="required")]
        public string FormName { get; set; }
        [InterColumn(ColumnName = "表单类型",ColumnType =Module.ColumnTypes.Select,DefaultValue ="1",DictionaryName = "Master.WorkFlow.Domains.FormType",Templet ="{{d.formType_display||''}}")]
        public FormType FormType { get; set; }
        /// <summary>
        /// 表单原html模板未经处理的
        /// </summary>
        [Description("表单内容")]
        public string FormContent { get; set; }
        /// <summary>
        /// 表单处理程序
        /// </summary>
        public string FormHandler { get; set; }
        [InterColumn(ColumnName = "排序",ColumnType =Module.ColumnTypes.Number,DisplayFormat ="0",DefaultValue ="1")]
        public int Sort { get; set; }
        [InterColumn(ColumnName = "有效", ColumnType = Module.ColumnTypes.Switch,DefaultValue ="true")]
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 是否系统表单
        /// </summary>
        [InterColumn(ColumnName = "系统", ColumnType = Module.ColumnTypes.Switch, DefaultValue = "false", IsShownInAdd = false, IsShownInEdit = false,Templet = "{{d.isSystemForm?'<span class=\"layui-badge\">系统</span>':'<span class=layui-badge layui-bg-cyan\">用户</span>'}}")]
        public bool IsSystemForm { get; set; } = false;
    }

    public enum FormType
    {        
        /// <summary>
        /// 设计器
        /// </summary>
        Designer=1,
        //纯Html
        Html = 2,
        //spread
        Spread =3
    }
}

using Abp.AutoMapper;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow
{
    [AutoMap(typeof(FlowScheme))]
    public class FlowSchemeDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>        
        public string SchemeCode { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>       
        public string SchemeName { get; set; }
        /// <summary>
        /// 流程分类
        /// </summary>
        public string SchemeType { get; set; }
        /// <summary>
        /// 流程内容版本
        /// </summary>
        public string SchemeVersion { get; set; }
        /// <summary>
        /// 流程内容
        /// </summary>
        public string SchemeContent { get; set; }
        /// <summary>
        /// 表单ID
        /// </summary>
        public int FlowFormId { get; set; }
        /// <summary>
        /// 模板权限类型：0完全公开,1指定部门/人员
        /// </summary>
        public int AuthorizeType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}

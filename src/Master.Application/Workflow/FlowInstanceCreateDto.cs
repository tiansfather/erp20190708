using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow
{
    /// <summary>
    /// 添加流程实例
    /// </summary>
    [AutoMap(typeof(FlowInstance))]
    public class FlowInstanceCreateDto
    {
        public int Id { get; set; }
        public int? FlowFormId { get; set; }
        public int? FlowSchemeId { get; set; }
        public string FormData { get; set; }
    }
}

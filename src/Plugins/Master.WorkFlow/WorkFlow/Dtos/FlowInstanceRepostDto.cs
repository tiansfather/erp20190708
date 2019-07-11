using Abp.AutoMapper;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Dtos
{
    /// <summary>
    /// 重发流程实例
    /// </summary>
    public class FlowInstanceRepostDto
    {
        public int Id { get; set; }
        public string FormData { get; set; }
    }
}

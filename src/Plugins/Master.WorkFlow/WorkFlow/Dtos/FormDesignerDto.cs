using Abp.AutoMapper;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Dtos
{
    [AutoMap(typeof(FlowForm))]
    public class FormDesignerDto
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public List<FormDesignerItem> Datas { get; set; }
    }
}

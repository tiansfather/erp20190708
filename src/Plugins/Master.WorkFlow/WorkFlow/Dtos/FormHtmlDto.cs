using Abp.AutoMapper;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Dtos
{
    [AutoMap(typeof(FlowForm))]
    public class FormHtmlDto
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FormContent { get; set; }
    }
}

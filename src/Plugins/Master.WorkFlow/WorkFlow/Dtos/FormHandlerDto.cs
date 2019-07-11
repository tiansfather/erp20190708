using Abp.AutoMapper;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Dtos
{
    [AutoMap(typeof(FlowForm))]
    public class FormHandlerDto
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FormHandler { get; set; }
    }
}

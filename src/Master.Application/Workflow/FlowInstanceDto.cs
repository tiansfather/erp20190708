﻿using Abp.AutoMapper;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow
{
    [AutoMap(typeof(FlowInstance))]
    public class FlowInstanceDto
    {
        public int Id { get; set; }
        public string ActivityId { get; set; }
        public string SchemeContent { get; set; }
        public string FormData { get; set; }
        public string MakerList { get; set; }
    }
}
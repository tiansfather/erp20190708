﻿using Abp.AutoMapper;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.BaseTrees
{
    [AutoMap(typeof(BaseTree))]
    public class BaseTreeDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name
        {
            get
            {
                return DisplayName+(Nature==0?"[外]":"内");
            }
        }
        public string DisplayName { get; set; }
        public string BriefCode { get; set; }
        public int Sort { get; set; }
        public int Nature { get; set; }
        public string Remarks { get; set; }
        public string Discriminator { get; set; }
        public string Category { get; set; }
    }
}

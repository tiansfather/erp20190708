using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Units
{
    [AutoMap(typeof(Unit))]
    public class UnitDto
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public string UnitTypeDisplayName { get; set; }

    }
}

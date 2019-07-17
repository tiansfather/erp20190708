using Abp.Domain.Entities.Auditing;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    public class MaterialBuy: CreationAuditedEntity<int>
    {
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
    }
}

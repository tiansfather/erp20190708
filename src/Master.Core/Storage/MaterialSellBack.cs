using Abp.Domain.Entities.Auditing;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    public class MaterialSellBack : CreationAuditedEntity<int>
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        public int BackNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}

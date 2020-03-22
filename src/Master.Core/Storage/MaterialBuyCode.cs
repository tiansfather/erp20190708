using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    public class MaterialBuyCode: CreationAuditedEntity<int>
    {
        public int MaterialBuyId { get; set; }
        public virtual MaterialBuy MaterialBuy { get; set; }
        public decimal? CodeStartNumber { get; set; }
        public decimal? CodeEndNumber { get; set; }
    }
}

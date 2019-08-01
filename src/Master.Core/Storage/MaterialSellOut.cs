using Abp.Domain.Entities.Auditing;
using Abp.Specifications;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 销售出库
    /// </summary>
    public class MaterialSellOut : CreationAuditedEntity<int>
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }


}

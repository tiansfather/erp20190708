using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 订货商品汇总Dto
    /// </summary>
    public class MaterialSellSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaterialTypeName { get; set; }
        public string Specification { get; set; }
        public string MeasureMentUnit { get; set; }
        public string MaterialNature { get; set; }
        public decimal SellNumber { get; set; }
        public decimal OutNumber { get; set; }
        public decimal StoreNumber { get; set; }
    }
}

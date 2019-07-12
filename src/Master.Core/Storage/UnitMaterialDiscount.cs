using Abp.Domain.Entities.Auditing;
using Master.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 代理商商品折扣
    /// </summary>
    public class UnitMaterialDiscount : CreationAuditedEntity<int>
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public UnitDiscount UnitDiscount { get; set; }
        public UnitSellMode UnitSellMode { get; set; }
    }

    public enum UnitDiscount
    {
        默认,
        折扣1,
        折扣2,
        折扣3
    }

    public enum UnitSellMode
    {
        始终销售,
        售完为止,
        停止销售
    }
}

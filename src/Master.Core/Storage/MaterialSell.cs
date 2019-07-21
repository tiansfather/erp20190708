using Abp.Domain.Entities.Auditing;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Storage
{
    public class MaterialSell : CreationAuditedEntity<int>
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int SellNumber { get; set; }
        /// <summary>
        /// 已出库数量
        /// </summary>
        public int OutNumber { get; set; }
        public int BackNumber { get; set; }
        /// <summary>
        /// 是否购物车商品
        /// </summary>
        public bool IsInCart { get; set; }
        /// <summary>
        /// 可退货数量
        /// </summary>
        [NotMapped]
        public int CanBackNumber
        {
            get
            {
                return SellNumber - BackNumber;
            }
        }
    }
}

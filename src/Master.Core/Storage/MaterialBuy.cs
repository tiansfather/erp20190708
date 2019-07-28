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
    public class MaterialBuy: CreationAuditedEntity<int>
    {
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public int FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        public int BuyNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int BackNumber { get; set; }
        public string FeatureCode { get; set; }
        public string CodeStartNumber { get; set; }
        public string CodeEndNumber { get; set; }
        [NotMapped]
        public int CanBackNumber
        {
            get
            {
                return BuyNumber - BackNumber;
            }
        }
    }

    public class MaterialBuySpecification : Specification<MaterialBuy>
    {
        private int _unitId;
        private DateTime _startdate;
        public MaterialBuySpecification(int unitId,DateTime startDate)
        {
            _unitId = unitId;
            _startdate = startDate;
        }
        public override Expression<Func<MaterialBuy, bool>> ToExpression()
        {
            return o => o.FlowSheet.UnitId == _unitId && o.CreationTime > _startdate && o.FlowSheet.SheetNature == SheetNature.正单;
        }
    }
}

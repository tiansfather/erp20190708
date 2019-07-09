using Abp.Domain.Entities;
using Master.Entity;
using Master.Projects;
using Master.Units;
using System;
using System.Collections.Generic;
using System.Text;
using Master.Module.Attributes;
using Abp.Domain.Entities.Auditing;
using Master.MultiTenancy;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Orders
{
    [InterModule("合同管理")]
    /// <summary>
    /// 订单实体,一个订单可以包含多个项目
    /// </summary>
    public class Order : BaseFullEntityWithTenant, IHaveStatus,IPassivable
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [InterColumn(ColumnName = "合同编码", Sort = 0)]
        public string OrderSN { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        [InterColumn(ColumnName = "合同名称", Sort = 1)]
        public string OrderName { get; set; }
        [InterColumn(ColumnName = "客户", ColumnType = Module.ColumnTypes.Reference, RelativeDataType = Module.RelativeDataType.Module, RelativeDataString = nameof(Unit), MaxReferenceNumber = "1", ReferenceItemTpl = "unitName", ReferenceSearchColumns = "unitName", ReferenceSearchWhere = "{\"where\":\"UnitNature = 1 or UnitNature = 3\"}", Sort = -1, DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display==null?'':d.unitId_display}}")]
        public int? UnitId { get; set; }
        
        [ForeignKey("OrderId")]
        public virtual ICollection<Project> Projects { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual Unit Unit { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

    
}

using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Storage
{
    [InterModule("散装组合配置", BaseType = typeof(Material))]
    public class MaterialDIY : Material
    {
        [NotMapped]
        [InterColumn(ColumnName ="配置状态",ValuePath ="Property",Templet ="{{d.isDiyed?'已配置':'未配置'}}",Sort =8)]
        public virtual bool IsDiyed{get;set;}
    }

    /// <summary>
    /// 组装信息
    /// </summary>
    public class DIYInfo
    {
        public int MaterialId { get; set; }
        public int Number { get; set; }
    }
}

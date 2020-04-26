using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Storage
{
    [InterModule("散装组合配置", BaseType = typeof(Material),GenerateOperateColumn =true)]
    public class MaterialDIY : Material
    {
        [NotMapped]
        [InterColumn(ColumnName ="配置状态",Templet ="{{d.isDiyed?'已配置':'<font color=\"red\">未配置</font>'}}",Sort =8, EnableDataFilter = true)]
        public override bool IsDiyed{get;set;}
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

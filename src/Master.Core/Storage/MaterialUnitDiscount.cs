using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    [InterModule("折扣与销售方式", BaseType = typeof(Material),GenerateOperateColumn =true)]
    public class MaterialUnitDiscount : Material
    {
    }
}

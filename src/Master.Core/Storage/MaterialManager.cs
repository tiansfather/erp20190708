using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialManager : ModuleServiceBase<Material, int>
    {
        public override Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var material = entity as Material;
            if (moduleInfo.ModuleKey == nameof(MaterialDIY))
            {
                data["IsDiyed"] = material.DIYInfo.Count > 0;
            }
            return base.FillEntityDataAfter(data, moduleInfo, entity);
        }
    }
}

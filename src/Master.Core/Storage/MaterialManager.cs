using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class MaterialManager : ModuleServiceBase<Material, int>
    {
        public override async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var material = entity as Material;
            //if (moduleInfo.ModuleKey == nameof(MaterialDIY))
            //{
            //    data["IsDiyed"] = material.DIYInfo.Count > 0;
            //}
            //加入库存
            var storeCountInfo = await Resolve<StoreMaterialManager>().GetAll().Where(o => o.MaterialId == material.Id)
                .Select(o => new
                {
                    o.StoreId,
                    o.Number
                })
                .ToListAsync();
            data["StoreCount"] = storeCountInfo;
            data["TotalCount"] = storeCountInfo.Sum(o => o.Number);
        }
    }
}

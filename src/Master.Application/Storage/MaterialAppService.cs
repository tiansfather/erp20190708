using Abp.Authorization;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialAppService : ModuleDataAppServiceBase<Material, int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        protected override string ModuleKey()
        {
            return nameof(Material);
        }

        protected override async Task<IQueryable<Material>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Material> query)
        {
            if (searchKeys.ContainsKey("MaterialTypeId"))
            {
                if (int.TryParse(searchKeys["MaterialTypeId"], out var materialTypeId))
                {
                    if (materialTypeId == -1)
                    {
                        query = query.Where(o => o.MaterialTypeId == null);
                    }
                    else
                    {
                        var materialType = await BaseTreeManager.GetByIdFromCacheAsync(materialTypeId);
                        //所有子类的id集合
                        var childIds = (await BaseTreeManager.FindChildrenAsync(materialTypeId, materialType.Discriminator, true)).Select(o => o.Id);
                        query = query.Where(o => o.MaterialTypeId == materialTypeId || o.MaterialTypeId != null && childIds.Contains(o.MaterialTypeId.Value));
                    }

                }
            }
            return query;
        }
    }
}

using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class StoreAppService : ModuleDataAppServiceBase<Store, int>
    {
        protected override string ModuleKey()
        {
            return nameof(Store);
        }
        /// <summary>
        /// 获取所有有效仓库
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetAllStores()
        {
            return (await Manager.GetAllList())
                .Where(o => o.IsActive)
                .Select(o => new {
                    o.Id,
                    o.Name
                });
        }
    }
}

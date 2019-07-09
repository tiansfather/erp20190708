using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    [AbpAuthorize]
    public class StoreAppService : ModuleDataAppServiceBase<Store, int>
    {
        protected override string ModuleKey()
        {
            return nameof(Store);
        }
    }
}

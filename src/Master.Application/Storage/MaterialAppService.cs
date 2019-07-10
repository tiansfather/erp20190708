using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialAppService : ModuleDataAppServiceBase<Material, int>
    {
        protected override string ModuleKey()
        {
            return nameof(Material);
        }
    }
}

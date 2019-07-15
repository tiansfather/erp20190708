using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    [AbpAuthorize]
    public class StoreMaterialHistoryAppService : ModuleDataAppServiceBase<StoreMaterialHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(StoreMaterialHistory);
        }
    }
}

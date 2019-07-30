using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    public class FeeAccountHistoryAppService : ModuleDataAppServiceBase<FeeAccountHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(FeeAccountHistory);
        }
    }
}

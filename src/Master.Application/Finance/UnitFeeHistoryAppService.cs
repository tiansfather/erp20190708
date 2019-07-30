using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    public class UnitFeeHistoryAppService : ModuleDataAppServiceBase<UnitFeeHistory, int>
    {
        protected override string ModuleKey()
        {
            return nameof(UnitFeeHistory);
        }
    }
}

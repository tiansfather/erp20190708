using Abp.Authorization;
using Abp.UI;
using Master.Dto;
using Master.Finance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class FeeCheckAppService : ModuleDataAppServiceBase<FeeCheck, int>
    {
        protected override string ModuleKey()
        {
            return nameof(FeeCheck);
        }

    }
}

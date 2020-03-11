using Abp.Authorization;
using Abp.AutoMapper;
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

        public virtual async Task<object> GetAllAvailable()
        {
            return (await Manager.GetAll().Where(o => o.CheckStatus == CheckStatus.收入 || o.CheckStatus==CheckStatus.支出退票)
                .ToListAsync())
                .MapTo<List<FeeCheckDto>>();
        }

        public override async Task<object> GetById(int primary)
        {
            var feeCheck=await Manager.GetAll().Include(o => o.InFlowSheet).Include(o => o.OutFlowSheet)
                .Where(o => o.Id == primary).SingleAsync();
            return new
            {
                feeCheck.CheckNumber,
                feeCheck.CheckFee,
                feeCheck.CheckCompany,
                feeCheck.CheckBank,
                CheckDate=feeCheck.CheckDate.ToString("yyyy-MM-dd"),
                feeCheck.CheckDaySpan,
                CheckStatus=feeCheck.CheckStatus.ToString(),
                InFlowSheetSN=feeCheck.InFlowSheet?.SheetSN,
                OutFlowSheetSN=feeCheck.OutFlowSheet?.SheetSN,
                feeCheck.InFlowSheetId,
                feeCheck.OutFlowSheetId
            };
        }
    }
}

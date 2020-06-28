using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Web.Models;
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
    public class FeeAccountAppService : ModuleDataAppServiceBase<FeeAccount, int>
    {
        public FeeAccountHistoryManager FeeAccountHistoryManager { get; set; }
        protected override string ModuleKey()
        {
            return nameof(FeeAccount);
        }

        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            await (Manager as FeeAccountManager).BuildDefaultAccount();
            return await base.GetPageResult(request);
        }

        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            //todo:需要验证删除可行性
            if((await Manager.GetAll().CountAsync(o=>ids.Contains(o.Id) && (o.Name=="现金账户" || o.Name == "支票账户" || o.Name=="过账账户")) )> 0)
            {
                throw new UserFriendlyException("现金账户、支票账户、过账账户无法删除");
            }
            await base.DeleteEntity(ids);
        }

        /// <summary>
        /// 获取正常账号，除现金和支票
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetNormalAccounts()
        {
            var allAccounts = await Manager.GetAllList();
            return allAccounts.Where(o => o.Name != "现金账户" && o.Name != "支票账户" && o.Name!="过账账户")
                .Select(o => new
                {
                    o.Id,
                    o.Name
                });
        }

        /// <summary>
        /// 获取所有账户
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetAllAccounts()
        {
            var allAccounts = await Manager.GetAllList();
            return allAccounts
                .Select(o => new
                {
                    o.Id,
                    o.Name
                });
        }
        /// <summary>
        /// 获取账户进出统计
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [DontWrapResult]
        public virtual async Task<object> GetSummary(DateTime? startDate,DateTime? endDate)
        {
            var result = new List<object>();
            var allAccounts = await Manager.GetAllList();
            var totalInSum = 0M;
            var totalOutSum = 0M;
            foreach(var account in allAccounts)
            {
                var query = FeeAccountHistoryManager.GetAll().Where(o => o.FeeAccountId == account.Id)
                    .WhereIf(startDate.HasValue, o => o.CreationTime >= startDate.Value)
                    .WhereIf(endDate.HasValue, o => o.CreationTime < endDate.Value.AddDays(1));
                var totalIn = await query.Where(o => o.Fee > 0).SumAsync(o => o.Fee);
                var totalOut= await query.Where(o => o.Fee < 0).SumAsync(o => o.Fee);
                totalInSum += totalIn;
                totalOutSum += totalOut;
                result.Add(new
                {
                    account.Name,
                    totalIn=totalIn.ToString("N2"),
                    totalOut=totalOut.ToString("N2")
                });
            }
            //增加合计
            result.Add(new
            {
                Name="合计",
                totalIn=totalInSum,
                totalOut=totalOutSum
            });
            return new ResultDto() {
                data=result
            }
                ;
        }
    }
}

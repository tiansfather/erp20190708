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
    public class FeeAccountAppService : ModuleDataAppServiceBase<FeeAccount, int>
    {
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
            if((await Manager.GetAll().CountAsync(o=>ids.Contains(o.Id) && (o.Name=="现金账户" || o.Name == "支票账户")) )> 0)
            {
                throw new UserFriendlyException("现金账户及支票账户无法删除");
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
            return allAccounts.Where(o => o.Name != "现金账户" && o.Name != "支票账户")
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
    }
}

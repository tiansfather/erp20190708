﻿using Abp.Authorization;
using Abp.UI;
using Master.Dto;
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
    }
}

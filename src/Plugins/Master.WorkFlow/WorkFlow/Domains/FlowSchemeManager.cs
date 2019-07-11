using Abp.UI;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Abp.Events.Bus.Entities;
using Microsoft.EntityFrameworkCore;
using Abp.Events.Bus.Handlers;

namespace Master.WorkFlow.Domains
{
    public class FlowSchemeManager : ModuleServiceBase<FlowScheme, int>
    {
        public async override Task ValidateEntity(FlowScheme entity)
        {
            if (string.IsNullOrEmpty(entity.SchemeName))
            {
                throw new UserFriendlyException(L("流程名称不能为空"));
            }

            //不允许相同数据存在
            if (entity.Id > 0 && await Repository.CountAsync(o => o.SchemeName == entity.SchemeName && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同流程名称已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => o.SchemeName == entity.SchemeName) > 0)
            {
                throw new UserFriendlyException(L("相同流程名称已存在"));
            }
            await base.ValidateEntity(entity);
        }

        
    }
}

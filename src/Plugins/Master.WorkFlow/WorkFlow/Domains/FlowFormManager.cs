using Abp.UI;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using System.Linq;

namespace Master.WorkFlow.Domains
{
    public class FlowFormManager : ModuleServiceBase<FlowForm, int>
    {
        public async override Task ValidateEntity(FlowForm entity)
        {
            if (string.IsNullOrEmpty(entity.FormName) || string.IsNullOrEmpty(entity.FormKey))
            {
                throw new UserFriendlyException(L("表单名称及表单标志不能为空"));
            }

            //不允许相同表单名称及表单标志存在
            if (entity.Id > 0 && await Repository.CountAsync(o => (o.FormName == entity.FormName || o.FormKey == entity.FormKey) && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同表单名称或表单标志已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => o.FormName == entity.FormName || o.FormKey == entity.FormKey) > 0)
            {
                throw new UserFriendlyException(L("相同表单名称或表单标志已存在"));
            }
            await base.ValidateEntity(entity);
        }

        public async Task<FlowForm> GetByKey(string formKey)
        {
            return await GetAll().Where(o => o.FormKey == formKey).FirstOrDefaultAsync();
        }
    }
}

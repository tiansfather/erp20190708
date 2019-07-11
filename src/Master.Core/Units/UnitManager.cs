using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Master.Domain;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace Master.Units
{
    public class UnitManager: ModuleServiceBase<Unit, int>,IUnitManager
    {
        /// <summary>
        /// 通过缓存获取所有往来单位信息
        /// </summary>
        /// <returns></returns>
        //public virtual async Task<List<Unit>> GetAllList()
        //{
        //    var tenantId = CurrentUnitOfWork.GetTenantId();
        //    var key = "Unit" + "@" + (tenantId ?? 0);
        //    return await CacheManager.GetCache<string, List<Unit>>("Unit")
        //        .GetAsync(key, async () => { return await GetAll().ToListAsync(); });
        //}
        public async override Task ValidateEntity(Unit entity)
        {
            if (string.IsNullOrEmpty(entity.BriefName) || string.IsNullOrEmpty(entity.UnitName))
            {
                throw new UserFriendlyException(L("单位名称及单位简称不能为空"));
            }

            //重名验证
            if (entity.Id > 0 && await Repository.CountAsync(o => (o.UnitName == entity.UnitName || o.BriefName==entity.BriefName) && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同单位名称或单位简称已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => (o.UnitName == entity.UnitName || o.BriefName == entity.BriefName)) > 0)
            {
                throw new UserFriendlyException(L("相同单位名称或单位简称已存在"));
            }
            await base.ValidateEntity(entity);
        }
        

        
        //public void HandleEvent(EntityChangedEventData<Unit> eventData)
        //{
        //    var key = "Unit" + "@" + eventData.Entity.TenantId;
        //    CacheManager.GetCache<string, List<Unit>>("Unit").Remove(key);
        //}
    }
}

using Abp.Extensions;
using Abp.Reflection;
using Abp.UI;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Master.Entity;

namespace Master.Resources
{
    public class ResourceManager : DomainServiceBase<Resource, int>
    {
        #region 标记
        /// <summary>
        /// 获取对应的实体标记
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<FlagInfo>> GetFlagInfosByType(string type)
        {
            var destinationType = Resolve<ITypeFinder>().Find(o => o.FullName == type).FirstOrDefault();
            if (destinationType == null)
            {
                throw new UserFriendlyException("对应类型不存在");
            }

            //从缓存中获取
            var cacheKey = $"{type}@{AbpSession.TenantId ?? 0}";//缓存键

            return await CacheManager.GetCache<string, IEnumerable<FlagInfo>>("FlagInfos").GetAsync(cacheKey, async key =>
            {
                var result = new List<FlagInfo>();
                //先加入Host的标签
                var hostResource = await GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.TenantId == null && o.ResourceName == type && o.ResourceType == "Flags").FirstOrDefaultAsync();
                if (hostResource != null)
                {
                    result.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlagInfo>>(hostResource.ResourceContent));
                }
                //如果是账套登录，再加入对应账套的标签
                if (AbpSession.TenantId.HasValue)
                {
                    var tenantResource = await GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId.Value && o.ResourceName == type && o.ResourceType == "Flags").FirstOrDefaultAsync();
                    if (tenantResource != null)
                    {
                        result.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<FlagInfo>>(tenantResource.ResourceContent));
                    }
                }
                return result;
            });

            
        }
        /// <summary>
        /// 设置实体标记
        /// </summary>
        /// <param name="type"></param>
        /// <param name="flagInfos"></param>
        /// <returns></returns>
        public virtual async Task SetFlagInfosByType(string type, IEnumerable<FlagInfo> flagInfos)
        {
            var resource = await GetAll().Where(o => o.ResourceName == type && o.ResourceType == "Flags" ).FirstOrDefaultAsync();
            if (resource == null)
            {
                resource = new Resource()
                {
                    ResourceName = type,
                    ResourceType = "Flags",
                    TenantId=AbpSession.TenantId
                };
            }
            foreach(var flagInfo in flagInfos)
            {
                //如果显示名称为空则使用Name
                if (flagInfo.Name.IsNullOrEmpty())
                {
                    flagInfo.Name = flagInfo.DisplayName;
                }
            }
            resource.ResourceContent = Newtonsoft.Json.JsonConvert.SerializeObject(flagInfos);

            await SaveAsync(resource);

            //清除对应的缓存
            if (AbpSession.TenantId.HasValue)
            {
                var cacheKey = $"{type}@{AbpSession.TenantId ?? 0}";//缓存键
                await CacheManager.GetCache<string, IEnumerable<FlagInfo>>("FlagInfos").RemoveAsync(cacheKey);
            }
            else
            {
                //如果是Host登录，清空所有缓存
                await CacheManager.GetCache<string, IEnumerable<FlagInfo>>("FlagInfos").ClearAsync();
            }
        } 

        /// <summary>
        /// 获取实体中已经打上的标记
        /// </summary>
        /// <param name="type"></param>
        /// <param name="haveStatusEntity"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<FlagInfo>> GetCheckedFlagInfos(string type,IHaveStatus haveStatusEntity)
        {
            var result = new List<FlagInfo>();
            var flagInfos = await GetFlagInfosByType(type);
            foreach(var flagInfo in flagInfos)
            {
                if (haveStatusEntity.HasStatus(flagInfo.Name))
                {
                    result.Add(flagInfo);
                }
            }
            return result;
        }
        #endregion
    }
}

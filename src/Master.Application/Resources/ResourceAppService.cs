using Abp.Authorization;
using Abp.AutoMapper;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.Reflection;
using Abp.UI;
using Master.Domain;
using Master.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Resources
{
    [AbpAuthorize]
    public class ResourceAppService : MasterAppServiceBase<Resource, int>
    {

        #region 标记定义及获取
        /// <summary>
        /// 获取在实体类型上定义的所有标记
        /// </summary>
        /// <param name="type">实体的完整类名:Master.Domains.User</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<FlagInfo>> GetFlagInfosByType(string type)
        {
            return await (Manager as ResourceManager).GetFlagInfosByType(type);
        } 
        /// <summary>
        /// 设置实体的标记1111
        /// </summary>
        /// <param name="type"></param>
        /// <param name="flagInfos"></param>
        /// <returns></returns>
        public virtual async Task SetFlagInfosByType(string type, IEnumerable<FlagInfo> flagInfos)
        {
            var manager = Manager as ResourceManager;
            var myFlagInfos = flagInfos.ToList();
            //如果是账套登录，去除提交过来的标记中的系统标记
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Tenant)
            {
                myFlagInfos.RemoveAll(o => o.System);
            }
            //标记是否有系统标记，即Host建立
            foreach(var flagInfo in flagInfos)
            {
                flagInfo.System = AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host;
            }
            await manager.SetFlagInfosByType(type, flagInfos);
        }
        #endregion

        #region 实体已绑定标记及提交标记
        /// <summary>
        /// 获取实体绑定的标记名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<string>> GetBindedFlagNames(int id,string type)
        {
            return (await GetBindedFlags(id, type)).Select(o => o.Name);
        }
        /// <summary>
        /// 获取实体上绑定的标记
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<FlagInfo>> GetBindedFlags(int id, string type)
        {
            var entityTypes = Resolve<ITypeFinder>().Find(o => o.FullName == type);
            if (entityTypes == null)
            {
                throw new UserFriendlyException($"未找到对应实体{type}");
            }
            var entityType = entityTypes[0];
            //获取Status字段并反序列化
            var status = await Resolve<IDynamicQuery>().SingleOrDefaultAsync<string>($"select status from {entityType.Name} where id={id} and tenantId={AbpSession.TenantId}");
            if (status.IsNullOrEmpty())
            {
                return new List<FlagInfo>();
            }
            else
            {
                var statusArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(status);
                var allFlags = await GetFlagInfosByType(type);
                return allFlags.Where(o => statusArr.Contains(o.Name));
            }
        }
        /// <summary>
        /// 设置实体绑定的标记
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="flagNames"></param>
        /// <returns></returns>
        public virtual async Task SetBindedFlags(int id,string type,IEnumerable<string> flagNames)
        {
            var entityTypes = Resolve<ITypeFinder>().Find(o => o.FullName == type);
            if (entityTypes == null)
            {
                throw new UserFriendlyException($"未找到对应实体{type}");
            }
            var entityType = entityTypes[0];
            //获取Status字段并反序列化
            var status = await Resolve<IDynamicQuery>().SingleOrDefaultAsync<string>($"select status from {entityType.Name} where id={id} and tenantId={AbpSession.TenantId}");
            List<string> statusArr = new List<string>();
            if (!status.IsNullOrEmpty())
            {
                statusArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(status);
            }

            var allFlags = await GetFlagInfosByType(type);
            foreach (var flag in allFlags)
            {
                //如果实体中没有，但提交过来有，则加入实体
                if (!statusArr.Contains(flag.Name) && flagNames.Contains(flag.Name))
                {
                    statusArr.Add(flag.Name);
                }
                //实体中有但提交过来没有，则删除
                else if(statusArr.Contains(flag.Name) && !flagNames.Contains(flag.Name))
                {
                    statusArr.Remove(flag.Name);
                }
            }
            //更新
            await Resolve<IDynamicQuery>().ExecuteAsync($"update {entityType.Name} set status='{Newtonsoft.Json.JsonConvert.SerializeObject(statusArr)}' where id={id}");
        }
        #endregion
    }
}

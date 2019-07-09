using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Application
{
    public class SystemLogManager:DomainServiceBase<SystemLog,int>
    {
        /// <summary>
        /// 增加实体变更日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityId"></param>
        /// <param name="logContent"></param>
        /// <returns></returns>
        public virtual async Task AddEntityLog<T>(int entityId,string logContent)
        {
            var log = new SystemLog()
            {
                LogName="EntityChange",
                LogGroup=typeof(T).FullName,
                LogContent=logContent,
                LogEntityIdentifier=entityId.ToString()
            };

            await InsertAsync(log);
        }

        /// <summary>
        /// 获取实体变更日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<SystemLog>> GetEntityLogs<T>(int entityId)
        {
            return await GetAll()
                .Where(o => o.LogName == "EntityChange")
                .Where(o => o.LogGroup == typeof(T).FullName)
                .Where(o => o.LogEntityIdentifier == entityId.ToString())
                .ToListAsync();
        }
    }
}

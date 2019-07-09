using Abp.UI;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    public class StoreManager : ModuleServiceBase<Store, int>
    {
        /// <summary>
        /// 仓库名字不能一样
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task ValidateEntity(Store entity)
        {
            
            //重名验证
            if (entity.Id > 0 && await Repository.CountAsync(o => (o.Name == entity.Name) && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同仓库名称已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => (o.Name == entity.Name )) > 0)
            {
                throw new UserFriendlyException(L("相同仓库名称已存在"));
            }
        }
    }
}

using Abp.UI;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class FeeAccountManager : ModuleServiceBase<FeeAccount, int>
    {
        /// <summary>
        /// 仓库名字不能一样
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task ValidateEntity(FeeAccount entity)
        {
            //重名验证
            if (entity.Id > 0 && await Repository.CountAsync(o => (o.Name == entity.Name) && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同账户名称已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => (o.Name == entity.Name)) > 0)
            {
                throw new UserFriendlyException(L("相同账户名称已存在"));
            }
        }

        /// <summary>
        /// 构建默认账套
        /// </summary>
        /// <returns></returns>
        public virtual async Task BuildDefaultAccount()
        {
            var accounts = await GetAllList();
            if (accounts.Count == 0)
            {
                await InsertAsync(new FeeAccount()
                {
                    Name = "支票账户",
                    IsActive = true,
                });
                await InsertAsync(new FeeAccount()
                {
                    Name = "现金账户",
                    IsActive = true
                });
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}

using Abp.UI;
using Master.Module;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class FeeAccountManager : ModuleServiceBase<FeeAccount, int>
    {
        public virtual async Task<FeeAccount> GetByName(string name)
        {
            await BuildDefaultAccount();
            return await GetAll().Where(o => o.Name == name).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 账号名字不能一样
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
                    Name = FeeAccount.StaticAccountName2,
                    IsActive = true,
                });
                await InsertAsync(new FeeAccount()
                {
                    Name = FeeAccount.StaticAccountName1,
                    IsActive = true
                });
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task BuildFeeHistory(FeeAccount account, int? unitId,decimal totalFee, FlowSheet flowSheet)
        {
            account.Fee += totalFee;
            var feeAccountHistory = new FeeAccountHistory()
            {
                FeeAccountId = account.Id,
                Fee = totalFee,
                UnitId=unitId,
                RemainFee = account.Fee + account.StartFee,//当前总结余,
                FeeDirection=totalFee>0?FeeDirection.进:FeeDirection.出,
                ChangeType = flowSheet.ChangeType,
                FlowSheetId = flowSheet.Id
            };

            await Resolve<FeeAccountHistoryManager>().InsertAsync(feeAccountHistory);
        }
    }
}

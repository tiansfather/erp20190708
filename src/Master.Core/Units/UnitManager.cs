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
using Master.WorkFlow;
using Master.Finance;
using Master.Entity;
using Master.Authentication;

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
        
        /// <summary>
        /// 往来单位金额变动
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="accountId"></param>
        /// <param name="totalFee"></param>
        /// <param name="flowSheet"></param>
        /// <returns></returns>
        public virtual async Task ChangeFee(int unitId,int? accountId,decimal totalFee,FlowSheet flowSheet,string targetCompany="")
        {
            //往来单位
            var unit = await GetByIdAsync(unitId);     
            // 记录变动明细
            await BuildFeeHistory(unit, totalFee, flowSheet);
            if (accountId.HasValue)
            {
                var feeAccountManager = Resolve<FeeAccountManager>();
                var account = await feeAccountManager.GetByIdAsync(accountId.Value);
                await feeAccountManager.BuildFeeHistory(account,unitId, totalFee, flowSheet,targetCompany);
            }
            
        }
        /// <summary>
        /// 构建往来单位变动明细
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="totalFee"></param>
        /// <param name="flowSheet"></param>
        /// <returns></returns>
        public virtual async Task BuildFeeHistory(Unit unit,decimal totalFee,FlowSheet flowSheet)
        {
            unit.Fee += totalFee;
            var unitFeeHistory = new UnitFeeHistory()
            {
                UnitId = unit.Id,
                Fee = totalFee,
                RemainFee = unit.Fee + unit.StartFee,//当前总结余,
                ChangeType = flowSheet.ChangeType,
                FlowSheetId = flowSheet.Id,
                RelCompanyName = flowSheet.GetPropertyValue<string>("RelCompanyName")
            };

            await Resolve<UnitFeeHistoryManager>().InsertAsync(unitFeeHistory);
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            if(await Resolve<UnitFeeHistoryManager>().GetAll().CountAsync(o => ids.Contains(o.UnitId)) > 0)
            {
                throw new UserFriendlyException("已产生往来金额的往来单位不能删除");
            }
            var userManager = Resolve<UserManager>();
            //先删除用户信息
            var users =await userManager.GetAll().Where(o => ids.ToList().Contains(o.UnitId.Value)).ToListAsync();
            foreach(var user in users)
            {
                await userManager.DeleteAsync(user);
            }
            await base.DeleteAsync(ids);
        }
        //public void HandleEvent(EntityChangedEventData<Unit> eventData)
        //{
        //    var key = "Unit" + "@" + eventData.Entity.TenantId;
        //    CacheManager.GetCache<string, List<Unit>>("Unit").Remove(key);
        //}
    }
}

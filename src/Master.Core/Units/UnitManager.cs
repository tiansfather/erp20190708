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
        /// <summary>
        /// 通过往来单位名称获取，若不存在，则新增
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public virtual async Task<Unit> GetByUnitNameOrInsert(string unitName,UnitNature unitNature=UnitNature.客户及供应商)
        {
            if (string.IsNullOrEmpty(unitName)) { return null; }
            var unit = await Repository.GetAll().Where(o => o.UnitName == unitName).FirstOrDefaultAsync();
            if (unit == null)
            {
                unit = new Unit()
                {
                    UnitName = unitName,
                    BriefName=unitName,
                    UnitNature= unitNature
                };
                //if(unitNature== UnitNature.供应商 || unitNature == UnitNature.客户及供应商)
                //{
                //    unit.SupplierType = "采购,加工";
                //}
                await InsertAsync(unit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return unit;
        }

        /// <summary>
        /// 查询往来单位时进行权限判定
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Unit> GetAll()
        {
            var query= base.GetAll();
            if (!PermissionChecker.IsGrantedAsync("Module.Unit.Button.ViewCustomer").Result)
            {
                query = query.Where(o => o.UnitNature != UnitNature.客户 && o.UnitNature != UnitNature.客户及供应商);
            }
            if (!PermissionChecker.IsGrantedAsync("Module.Unit.Button.ViewSupplier").Result)
            {
                query = query.Where(o => o.UnitNature != UnitNature.供应商 && o.UnitNature != UnitNature.客户及供应商);
            }
                return query;
        }

        //public void HandleEvent(EntityChangedEventData<Unit> eventData)
        //{
        //    var key = "Unit" + "@" + eventData.Entity.TenantId;
        //    CacheManager.GetCache<string, List<Unit>>("Unit").Remove(key);
        //}
    }
}

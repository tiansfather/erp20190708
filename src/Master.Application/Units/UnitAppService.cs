using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Master.Entity;
using Master.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Units
{
    [AbpAuthorize]
    public class UnitAppService : ModuleDataAppServiceBase<Unit, int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }

        /// <summary>
        /// 通过往来单位名称获取往来单位
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<List<UnitDto>> GetAll(string key,int take=200)
        {
            var query = Manager.GetAll();
            if (!key.IsNullOrEmpty())
            {
                query = query.Where(o => o.UnitName.Contains(key));
            }
            return (await query.OrderByDescending(o=>o.Id).Take(take).ToListAsync()).MapTo<List<UnitDto>>();
        }
        /// <summary>
        /// 通过往来单位性质获取往来单位
        /// </summary>
        /// <param name="unitNature">1:客户，2:供应商</param>
        /// <returns></returns>
        public virtual async Task<List<UnitDto>> GetAllByUnitNature(int unitNature,string key,int take=200)
        {
            var query = Manager.GetAll();
            if (unitNature == 0)
            {
                query = query.Where(o => o.UnitNature == UnitNature.代理商 );
            }else if (unitNature == 1)
            {
                query = query.Where(o => o.UnitNature == UnitNature.供应商);
            }

            if (!key.IsNullOrEmpty())
            {
                query = query.Where(o => o.UnitName.Contains(key));
            }
            return (await query.OrderByDescending(o => o.Id).Take(take).ToListAsync()).MapTo<List<UnitDto>>();
        }
       
        protected override string ModuleKey()
        {
            return nameof(Unit);
        }
        protected override async Task<IQueryable<Unit>> BuildKeywordQueryAsync(string keyword, IQueryable<Unit> query)
        {
            return query.Where(o=>o.UnitName.Contains(keyword));
        }
        protected override async Task<IQueryable<Unit>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Unit> query)
        {            

            //if (searchKeys.ContainsKey("UnitNature"))
            //{
            //    string UnitNaturename = searchKeys["UnitNature"].ToString();

            //    if(UnitNaturename=="供应商")
            //    {
            //        query = query.Where(o => o.UnitNature == UnitNature.供应商 || o.UnitNature == UnitNature.客户及供应商);
            //    }
            //    else if(UnitNaturename == "客户")
            //    {
            //        query = query.Where(o => o.UnitNature == UnitNature.客户 || o.UnitNature == UnitNature.客户及供应商);
            //    }
            //    else if (UnitNaturename == "客户及供应商")
            //    {
            //        query = query.Where(o =>  o.UnitNature == UnitNature.客户及供应商);
            //    }
            //}

            
            return query;
        }

        protected override object ResultConverter(Unit entity)
        {
            return new
            {
                entity.Id,
                entity.UnitName,
                entity.UnitNature,
                NowFee = entity.StartFee + entity.Fee
            };
        }
    }
}

﻿using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.UI;
using Master.Authentication;
using Master.Entity;
using Master.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Units
{
    [AbpAuthorize]
    public class UnitAppService : ModuleDataAppServiceBase<Unit, int>
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public UserManager UserManager { get; set; }
        public RoleManager RoleManager { get; set; }

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
        public virtual async Task<List<UnitDto>> GetAllByUnitNature(int? unitNature,string key,int take=1000)
        {
            var query = Manager.GetAll().Where(o=>o.IsActive);
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

        #region 往来单位发票
        public virtual async Task<object> GetInvoice(int id)
        {
            var unit = await Manager.GetByIdFromCacheAsync(id);
            return unit.UnitInvoice;
        }
        public virtual async Task SubmitInvoice(int id,UnitInvoice unitInvoice)
        {
            var unit = await Manager.GetByIdAsync(id);
            unit.UnitInvoice = unitInvoice;
            await Manager.UpdateAsync(unit);
        }
        public virtual async Task<object> GetInvoiceDetail(int id)
        {
            var unit = await Manager.GetByIdFromCacheAsync(id);
            return new
            {
                BuyUnitAddress=unit.Address,
                BuyUnitPhone=unit.Phone,
                BuyUnitName=unit.UnitInvoice.CompanyName,
                BuyUnitTaxNumber=unit.UnitInvoice.TaxNumber,
                BuyUnitBank=unit.UnitInvoice.Bank,
                BuyUnitBankAccount=unit.UnitInvoice.BankAccount
            };
        }
        #endregion

        #region 账户
        public virtual async Task<int> Init(string filePath=null)
        {
            filePath = filePath ?? "/init.xlsx";
            var dt=Common.ExcelHelper.ReadExcelToDataTable(Common.PathHelper.VirtualPathToAbsolutePath(filePath),out _);
            if (dt != null)
            {
                var addCount = 0;
                foreach(DataRow row in dt.Rows)
                {
                    var unit = new Unit()
                    {
                        IsActive = true,
                        UnitName = row["往来单位名称"].ToString(),
                        BriefName = row["往来单位名称"].ToString(),
                        UnitNature = row["往来单位类型"].ToString() == "代理商" ? UnitNature.代理商 : UnitNature.供应商,
                        UnitRank = 1,
                    };
                    if(Manager.Repository.Count(o=>o.UnitName==unit.UnitName) == 0)
                    {
                        await Manager.InsertAndGetIdAsync(unit);
                        await SubmitAccountInfo(unit.Id, row["登录账号"].ToString(), "88888888");
                        addCount++;
                    }
                    
                }
                return addCount;
            }
            return 0;
            
        }
        public virtual async Task<object> GetAccountInfo(int id){
            var user = await UserManager.GetAll().Where(o => o.UnitId == id).FirstOrDefaultAsync();
            return new
            {
                user?.UserName
            };
        }
        public virtual async Task SubmitAccountInfo(int id,string userName,string password)
        {
            var users = await UserManager.GetAll().Where(o => o.UnitId == id).ToListAsync();
            if (users.Count==0)
            {
                var unitRole = await RoleManager.GetAll().Where(o => o.Name == StaticRoleNames.Tenants.Unit).FirstOrDefaultAsync();
                //新增用户
                var newNames = new string[] { userName/*, userName + "1", userName + "2", userName + "3"*/ };
                foreach (var newName in newNames)
                {
                    var user = new User()
                    {
                        UserName = newName,
                        Name = newName,
                        IsActive = true,
                        TenantId = AbpSession.TenantId,
                        UnitId = id
                    };
                    await UserManager.InsertAsync(user);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    await UserManager.SetPassword(user, password);
                    await UserManager.SetRoles(user, new int[] { unitRole.Id });
                }
            }
            else
            {
                foreach (var user in users)
                {
                    user.UserName = userName;
                    await UserManager.SetPassword(user, password);
                }
                //修改
                //if (users[0].UserName != userName)
                //{
                //    throw new UserFriendlyException("暂不支持修改往来单位账号,此单位账号固定为"+ users[0].UserName);
                //}
                //else
                //{
                    
                //}
            }
        }
        #endregion
    }
}

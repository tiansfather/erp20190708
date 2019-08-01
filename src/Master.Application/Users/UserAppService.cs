using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Master.Authentication;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Entity;
using Master.Authentication.External;
using Master.Configuration;
using Abp.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Master.Users
{
    [AbpAuthorize]
    public class UserAppService:ModuleDataAppServiceBase<User,long>
    {
        private readonly IRepository<UserRole, int> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<UserLogin, int> _userLoginRepository;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;

        public MasterConfiguration MasterConfiguration { get; set; }

        public UserAppService(
            IRepository<UserRole, int> userRoleRepository,
            IRepository<Role, int> roleRepository,
            IRepository<UserLogin, int> userLoginRepository,
            IExternalAuthConfiguration externalAuthConfiguration
            )
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userLoginRepository = userLoginRepository;
            _externalAuthConfiguration = externalAuthConfiguration;
    }

        #region 表单提交



        public async override Task FormSubmit(FormSubmitRequestDto request)
        {
            switch (request.Action)
            {
                case "OffJob":
                    await OffJob(request);
                    break;
                case "Account":
                    await Account(request);
                    break;
                default:
                    await base.FormSubmit(request);
                    break;
            }

        }
        /// <summary>
        /// 帐号提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task Account(FormSubmitRequestDto request)
        {
            var manager = Manager as UserManager;
            var userId = Convert.ToInt64(request.Datas["ids"]);
            var user = await Repository.GetAsync(userId);
            if (user == null)
            {
                throw new UserFriendlyException(L("未找到对应员工"));
            }
            else
            {
                user.IsActive = request.Datas["isActive"] == "true";//是否启用账号

                if (user.IsActive)
                {
                    user.ToBeVerified = false;//启用账号后自动已审核
                    var username = request.Datas["userName"];
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new UserFriendlyException(L("用户名不能为空"));
                    }
                    var password = request.Datas["password"];
                    if (!string.IsNullOrEmpty(password))
                    {
                        await manager.SetPassword(user, password);
                    }
                    int[] roles = new int[] { };
                    if (!string.IsNullOrEmpty(request.Datas["roles"]))
                    {
                        roles = request.Datas["roles"].Split(',').ToList().ConvertAll(o=>int.Parse(o)).ToArray();
                    }
                    user.UserName = username;

                    //add 20181210  增加独立用户提交,此用户只能查看自己的信息
                    //removed 20190318
                    //user.IsSeparate= request.Datas["Separate"]=="1";
                    //add 20190118  增加标记获取
                    var statusDefinitions = MasterConfiguration.EntityStatusDefinitions.GetValueOrDefault(typeof(User));
                    if (statusDefinitions != null)
                    {
                        foreach (var statusDefinition in statusDefinitions)
                        {
                            if (request.Datas.ContainsKey(statusDefinition.Name) && request.Datas[statusDefinition.Name] == "1")
                            {
                                user.SetStatus(statusDefinition.Name);
                            }
                            else
                            {
                                user.RemoveStatus(statusDefinition.Name);
                            }
                        }
                    }
                    
                    await manager.SetRoles(user, roles);
                }
            }

        }

        /// <summary>
        /// 离职
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task OffJob(FormSubmitRequestDto request)
        {
            var ids = request.Datas["ids"].Split(',').ToList().ConvertAll(o => long.Parse(o));
            var offDate = Convert.ToDateTime(request.Datas["JobDateEnd"]);
            await (Manager as UserManager).OffJob(ids, offDate);
        }
        #endregion

        public virtual async Task<object> Get()
        {
            throw new UserFriendlyException("ErrorTest");
            //var user =await GetCurrentUserAsync();
            //user.SetPropertyValue("t1", new { Test="qqqqqqq"});
            //user.SetPropertyValue("t2", new UploadFileInfo() { FilePath = "tttttttt" });
            //return new
            //{
            //    T1 = user.GetPropertyValue("t1"),
            //    T2 = user.GetPropertyValue("t2"),
            //    T3 = user.GetPropertyValue<IDictionary<string,object>>("t2"),
            //    T2Obj = user.GetPropertyValue<UploadFileInfo>("t2")
            //};
        }

        [DontWrapResult]
        public virtual async Task<ResultPageDto> GetSimplePageResult(RequestPageDto requestPageDto)
        {
            var pageResult = await GetPageResultQueryable(requestPageDto);

            var data = await pageResult.Queryable
                .Select(o => new { o.Id, o.Name,o.UserName,o.PhoneNumber })
                .ToListAsync();

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }

        /// <summary>
        /// 获取在职、离职、全部数量统计
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetCountSummary()
        {
            var manager = Manager as UserManager;
            var query = manager.GetFilteredQuery();
            var inJobCount = await query.Where(o => o.JobDateEnd == null && (o.Status==null || !o.Status.Contains(User.Status_NotVerified))).CountAsync();
            var offJobCount = await query.Where(o => o.JobDateEnd != null && (o.Status == null || !o.Status.Contains(User.Status_NotVerified))).CountAsync();
            var accountCount = await query.Where(o => o.IsActive && (o.Status == null || !o.Status.Contains(User.Status_NotVerified))).CountAsync();
            var toBeVerifyCount = await query.Where(o => o.Status.Contains(User.Status_NotVerified)).CountAsync();
            var allCount = await query.CountAsync();

            return new {  allCount, inCount = inJobCount, offCount = offJobCount,  accountCount, toBeVerifyCount };
        }

        //[DontWrapResult]
        //public override async  Task<ResultPageDto> GetPageResult(RequestPageDto request)
        //{

        //    var pageResult = await GetPageResultQueryable(request);

        //    var data = (await pageResult.Queryable.Include(o=>o.Organization).ToListAsync())
        //        .Select(o => {
        //            var roles = UserManager.GetRolesAsync(o).Result;
        //            return new {o.Id, o.Name, o.UserName, o.IsActive, RoleName = string.Join(",", roles.Select(r => r.DisplayName)),OrganizationName=o.Organization?.DisplayName };
        //        });


        //    var result = new ResultPageDto()
        //    {
        //        code = 0,
        //        count = pageResult.RowCount,
        //        data = data
        //    };

        //    return result;
        //}


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oriPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual async Task ChangePassword(string oriPassword, string newPassword)
        {
            var user = await Repository.GetAsync(AbpSession.UserId.Value);
            var manager = Manager as UserManager;
            if (!await manager.CheckPasswordAsync(user, oriPassword))
            {
                throw new UserFriendlyException("原密码输入不正确");
            }

            await manager.SetPassword(user, newPassword);
        }

        /// <summary>
        /// 获取可登录的第三方信息
        /// </summary>
        /// <returns></returns>
        public virtual List<ExternalLoginProviderInfo> GetLoginProviders()
        {
            return _externalAuthConfiguration.Providers;
        }

        /// <summary>
        /// 获取用户绑定的所有第三方登录
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<string>> GetBindedLoginProviders(long? userId)
        {
            var user = await Repository.GetAllIncluding(o => o.Logins).SingleOrDefaultAsync(o => o.Id == (userId.HasValue ? userId.Value : AbpSession.UserId.Value));
            return user.Logins.Select(o => o.LoginProvider).ToList();
        }

        /// <summary>
        /// 解除第三方登录绑定
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task UnBindLogin(string provider,long? userId)
        {
            await _userLoginRepository.DeleteAsync(o => o.UserId ==(userId.HasValue?userId.Value:AbpSession.UserId.Value) && o.LoginProvider == provider);
        }
        /// <summary>
        /// 移除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task RemoveFromRole(long userId, int roleId)
        {
            var manager = Manager as UserManager;
            await manager.RemoveFromRoleAsync(userId, roleId);
        }
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task MoveIntoRole(long[] userIds,int roleId)
        {
            var manager = Manager as UserManager;
            foreach(var userId in userIds)
            {
                await manager.MoveIntoRoleAsync(userId, roleId);
            }
        }
        protected override string ModuleKey()
        {
            return nameof(User);
        }
        /// <summary>
        /// 用户通过第三方注册账套内部用户
        /// </summary>
        /// <param name="externalUserRegisterDto"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task ExternalUserRegister(ExternalUserRegisterDto externalUserRegisterDto)
        {
            var manager = Manager as UserManager;
            using (CurrentUnitOfWork.SetTenantId(externalUserRegisterDto.TenantId))
            {
                var userLoginInfo = new Microsoft.AspNetCore.Identity.UserLoginInfo(externalUserRegisterDto.LoginProvider, externalUserRegisterDto.ProviderKey, "");
                //判断此第三方账号是否已绑定过
                var user = await manager.FindAsync(userLoginInfo);
                if (user != null)
                {
                    throw new UserFriendlyException(L("此登录信息已绑定过系统账号"));
                }
                //判断对应手机号是否已存在
                if (await manager.GetAll().CountAsync(o => o.PhoneNumber == externalUserRegisterDto.Mobile) > 0)
                {
                    throw new UserFriendlyException(L("此手机号已被注册"));
                }
                user = new User()
                {
                    UserName = externalUserRegisterDto.Mobile,
                    PhoneNumber = externalUserRegisterDto.Mobile,
                    Name = externalUserRegisterDto.Name,
                    Sex=externalUserRegisterDto.Sex,
                    TenantId=externalUserRegisterDto.TenantId,
                    OrganizationId=externalUserRegisterDto.OrganizationId,
                    IsActive=false,
                    
                };
                user.ToBeVerified = true;//设置用户未审核
                //插入员工信息
                await manager.InsertAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();
                //设置角色
                await manager.SetRoles(user, externalUserRegisterDto.RoleIds);
                //设置密码
                await manager.SetPassword(user, externalUserRegisterDto.Password);
                //绑定第三方登录信息
                await manager.BindExternalLogin(user, userLoginInfo);
            }
        }
        /// <summary>
        /// 审核用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public virtual async Task VerifyUser(IEnumerable<long> userIds)
        {
            var users = await Manager.GetListByIdsAsync(userIds);
            foreach(var user in users)
            {
                user.ToBeVerified = false;
                user.IsActive = true;//审核后自动启用账号
            }
        }
        /// <summary>
        /// 通过姓名获取用户相关信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUserInfoByName(string name)
        {
            var user = await Manager.GetAll()
                .Where(o => o.Name == name)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            else
            {
                return new
                {
                    user.Name,
                    user.Id,
                    user.PhoneNumber,
                    user.Sex,
                    user.UserName,
                    user.WorkNumber
                };
            }
        }
        /// <summary>
        /// 设置用户电子签名
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public virtual async Task SetSignature(string signature)
        {
            
            var user = await GetCurrentUserAsync();
            if (!string.IsNullOrEmpty(user.Signature))
            {
                var fileManager = Resolve<IFileManager>();
                //移除原来图片路径及数据库记录
                var file = await fileManager.GetAll().Where(o => o.FilePath == user.Signature).FirstOrDefaultAsync();
                if (file != null)
                {
                    await fileManager.DeleteFile(file.Id);
                }
            }
            user.Signature = signature;
        }
        /// <summary>
        /// 获取当前登录token用于前台判断账号登出
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<string> GetCurrentToken()
        {
            if (!AbpSession.UserId.HasValue)
            {
                return string.Empty;
            }
            var user = await Manager.GetByIdAsync(AbpSession.UserId.Value);
            return user.GetData<string>("currentToken");
        }
    }
}

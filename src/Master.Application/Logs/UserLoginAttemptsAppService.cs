﻿
using Master.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Logs
{
    /// <summary>
    /// 登录日志的请求api
    /// </summary>
    public class UserLoginAttemptsAppService : MasterAppServiceBase<UserLoginAttempt, int>
    {
        ///// <summary>
        ///// 获取登录日志
        ///// </summary>
        ///// <returns></returns>
        //public async virtual Task<object> GetLoginMessage()
        //{
        //    var result = await Repository.GetAll().ToListAsync();

        //    return new { allCount = allStaffCount, inCount = inJobStaffCount, offCount = offJobStaffCount };
        //}
    }
}

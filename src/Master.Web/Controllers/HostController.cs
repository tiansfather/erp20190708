using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HostController : MasterControllerBase
    {
        public IActionResult Tenant()
        {
            return View();
        }

        /// <summary>
        /// 语言包管理
        /// </summary>
        /// <returns></returns>
        public IActionResult Language()
        {
            return View();
        }
    }
}
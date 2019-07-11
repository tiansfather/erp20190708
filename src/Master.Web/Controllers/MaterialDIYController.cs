using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class MaterialDIYController: MasterModuleControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

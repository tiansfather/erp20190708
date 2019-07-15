using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class MaterialController: MasterModuleControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StoreMaterial()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }
    }
}

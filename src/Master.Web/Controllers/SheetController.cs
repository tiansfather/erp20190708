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
    public class SheetController : MasterControllerBase
    {
        public IActionResult Show(string name)
        {
            ViewBag.Name = name;
            return View();
        }
    }
}
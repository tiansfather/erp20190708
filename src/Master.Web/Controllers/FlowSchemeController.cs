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
    public class FlowSchemeController : MasterModuleControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        public override IActionResult Add(string modulekey)
        {
            return Redirect("/FlowScheme/Design");
        }

        public async override Task<IActionResult> Edit(string modulekey, int data)
        {
            return Redirect("/FlowScheme/Design?data="+data);
        }

        public IActionResult Design(int? data)
        {
            return View();
        }

        public IActionResult NodeInfo()
        {
            return View();
        }
    }
}
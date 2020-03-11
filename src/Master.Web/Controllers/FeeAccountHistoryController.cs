using Abp.AspNetCore.Mvc.Authorization;
using Master.Authentication;
using Master.EntityFrameworkCore;
using Master.Module;
using Master.Projects;
using Master.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class FeeAccountHistoryController: MasterModuleControllerBase
    {
        
        public IActionResult Index()
        {
            return View();
        }

    }
}

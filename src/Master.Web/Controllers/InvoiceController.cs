using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    public class InvoiceController : MasterModuleControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
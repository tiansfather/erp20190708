using Abp.AspNetCore.Mvc.Authorization;
using Master.WorkFlow.Domains;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class FlowInstanceController : MasterModuleControllerBase
    {
        public FlowInstanceManager FlowInstanceManager { get; set; }
        public IActionResult Index()
        {            
            return View();
        }
        
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public  IActionResult Deal()
        {

            return View();
        }
    }
}

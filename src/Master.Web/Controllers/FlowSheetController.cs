using Abp.AspNetCore.Mvc.Authorization;
using Master.WorkFlow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class FlowSheetController : MasterModuleControllerBase
    {
        public FlowSheetManager FlowSheetManager { get; set; }
        /// <summary>
        /// 查看单据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IActionResult> SheetView(int data,string mode)
        {
            //var sheet = await FlowSheetManager.GetByIdFromCacheAsync(data);
            var sheet = await FlowSheetManager.GetAll().Include(o=>o.Unit).IgnoreQueryFilters().Where(o=>o.Id==data).FirstOrDefaultAsync();

            if (sheet == null)
            {
                return Content("未找到单据信息");
            }


            return Redirect($"/FlowForm/InstanceView?data={sheet.FlowInstanceId}&mode={mode}");
        }
    }
}

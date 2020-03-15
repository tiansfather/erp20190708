using Abp.Authorization;
using Master.Finance;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    [AbpAuthorize]
    public class HomeAppService:MasterAppServiceBase
    {
        public FlowSheetManager FlowSheetManager { get; set; }
        public InvoiceManager InvoiceManager { get; set; }
        /// <summary>
        /// 首页汇总数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetSummary()
        {
            //待审核实物订单
            var count1 = await FlowSheetManager.GetAll().Where(o => o.FormKey == "SDR" && o.OrderStatus == "待审核").CountAsync();
            //待审核报损报溢单
            var count2=await FlowSheetManager.GetAll().Where(o=>o.FormKey== "ICH" && o.OrderStatus == "待审核").CountAsync();
            //待审核发票
            var count3 = await InvoiceManager.GetAll().Where(o => o.InvoiceStatus == InvoiceStatus.待审核).CountAsync();
            return new
            {
                count1,count2,count3
            };
        }
    }
}

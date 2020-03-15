using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class InvoiceManager : ModuleServiceBase<Invoice, int>
    {
        private static object _obj = new object();
        public virtual string GenerateSheetSN()
        {
            lock (_obj)
            {
                //当天数量
                var todayNum = GetAll().IgnoreQueryFilters().Where(o =>  o.CreationTime.Year == DateTime.Now.Year && o.CreationTime.Month == DateTime.Now.Month && o.CreationTime.Day == DateTime.Now.Day && !string.IsNullOrEmpty(o.SheetSN)).Count();
                var newNum = todayNum + 1;

                var sheetSN = "INV" + DateTime.Now.ToString("yyyyMMdd") + newNum.ToString().PadLeft(4, '0');
                return sheetSN;
            }

        }
    }
    public class FlowSheetEventHandler :
        IAsyncEventHandler<EntityCreatedEventData<Invoice>>,
        ITransientDependency
    {
        private static object _obj = new object();
        public InvoiceManager InvoiceManager { get; set; }
        
        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEventData<Invoice> eventData)
        {
            var entity = eventData.Entity;
            if (string.IsNullOrEmpty(entity.SheetSN))
            {
                entity.SheetSN = InvoiceManager.GenerateSheetSN();
                await InvoiceManager.UpdateAsync(entity);
            }
        }
    }
}

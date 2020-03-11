using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    public class InvoiceAppService : ModuleDataAppServiceBase<Invoice, int>
    {
        protected override string ModuleKey()
        {
            return nameof(Invoice);
        }

        public virtual async Task<object> GetCountSummary()
        {
            var query = Manager.GetAll();
            var allCount = await query.CountAsync();
            var unVerifyCount = await query.Where(o => o.InvoiceStatus==InvoiceStatus.待审核).CountAsync();
            var verifiedCount = await query.Where(o => o.InvoiceStatus==InvoiceStatus.已审核).CountAsync();
            var closedCount = await query.Where(o => o.InvoiceStatus==InvoiceStatus.已关闭).CountAsync();

            return new { allCount, unVerifyCount, verifiedCount, closedCount };
        }

        public virtual async Task Verify(IEnumerable<int> ids)
        {
            var invoices = await Manager.GetListByIdsAsync(ids);
            foreach(var invoice in invoices)
            {
                invoice.InvoiceStatus = InvoiceStatus.已审核;
            }
        }
        public virtual async Task Close(IEnumerable<int> ids)
        {
            var invoices = await Manager.GetListByIdsAsync(ids);
            foreach (var invoice in invoices)
            {
                invoice.InvoiceStatus = InvoiceStatus.已关闭;
            }
        }
    }
}

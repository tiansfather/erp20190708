using Abp.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
        public override async Task<Dictionary<string,object>> GetPageSummary(IQueryable<Invoice> queryable)
        {
            var result = new Dictionary<string, object>();
            result.Add("增票",(await queryable.Where(o => o.Type == "增票").SumAsync(o => o.Fee)).ToString("0.00"));
            result.Add("普票",(await queryable.Where(o => o.Type == "普票").SumAsync(o => o.Fee)).ToString("0.00"));
            return result;
        }
        public virtual async Task Submit(InvoiceSubmitDto invoiceSubmitDto)
        {
            var user = await GetCurrentUserAsync();
            var invoices = invoiceSubmitDto.Items.MapTo<IEnumerable<Invoice>>().ToList();
            invoices.ForEach(o =>
            {
                o.UnitId = invoiceSubmitDto.UnitId;
                o.OrderType = user.IsCenterUser ? "中心端代为申请" : "代理商自助申请";
                Manager.InsertAsync(o).GetAwaiter().GetResult();
            });
        }
        public virtual async Task Update(InvoiceDto invoiceDto)
        {
            var invoice = await Manager.GetByIdAsync(invoiceDto.Id);
            invoiceDto.MapTo(invoice);
        }
        public virtual async Task<object> DoImport(string filePath)
        {
            var dt=Common.ExcelHelper.ReadExcelToDataTable(Common.PathHelper.VirtualPathToAbsolutePath(filePath),out var _);
            var result = new List<object>();
            foreach(DataRow row in dt.Rows)
            {
                var obj = new Dictionary<string, string>();
                foreach(DataColumn column in dt.Columns)
                {
                    obj.Add(ColumnNameConverter(column.ColumnName), row[column.ColumnName].ToString());
                }
                result.Add(obj);
            }
            return result;
        }
        private string ColumnNameConverter(string columnName)
        {
            var result = columnName;
            switch (columnName)
            {
                case "开票内容":
                    result = "Content";
                    break;
                case "开票金额":
                    result = "Fee";
                    break;
                case "单价":
                    result = "Price";
                    break;
                case "数量":
                    result = "Number";
                    break;
                case "税率":
                    result = "TaxRate";
                    break;
                case "品牌单位名称":
                    result = "SellUnitName";
                    break;
                case "购货单位名称":
                    result = "BuyUnitName";
                    break;
                case "税务登记号":
                    result = "BuyUnitTaxNumber";
                    break;
                case "单位地址":
                    result = "BuyUnitAddress";
                    break;
                case "电话":
                    result = "BuyUnitPhone";
                    break;
                case "开户银行":
                    result = "BuyUnitBank";
                    break;
                case "开户行账号":
                    result = "BuyUnitBankAccount";
                    break;
                case "备注":
                    result = "Remarks";
                    break;
            }
            return result;
        }
        public virtual async Task<object> GetCountSummary()
        {
            var query = Manager.GetAll();
            var allCount = await query.CountAsync();
            var unVerifyCount = await query.Where(o => o.InvoiceStatus== InvoiceStatus.待审核).CountAsync();
            var verifiedCount = await query.Where(o => o.InvoiceStatus== InvoiceStatus.已审核).CountAsync();
            var closedCount = await query.Where(o => o.InvoiceStatus== InvoiceStatus.已关闭).CountAsync();

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

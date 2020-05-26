using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Master.Dto;
using Master.Storage;
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
        /// <summary>
        /// 获取已开金额
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetUnitInvoicedFee(int unitId,DateTime? startDate,DateTime? endDate)
        {
            return await Manager.GetAll().Where(o=>o.InvoiceStatus==InvoiceStatus.已审核)
                .Where(o=>o.UnitId==unitId)
                .WhereIf(startDate.HasValue, o => o.CreationTime >= startDate.Value)
                .WhereIf(endDate.HasValue, o => o.CreationTime <= endDate.Value)
                .SumAsync(o => o.Fee);
        }
        /// <summary>
        /// 获取进货金额
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetUnitStoredFee(int unitId, DateTime? startDate, DateTime? endDate)
        {
            return await Resolve<MaterialSellOutManager>().GetAll().Where(o => o.UnitId == unitId)
                .Where(o => o.FlowSheet.SheetNature == WorkFlow.SheetNature.正单)
                .Where(o => o.FlowSheet.OrderStatus == null || (o.FlowSheet.OrderStatus != "待审核" && o.FlowSheet.OrderStatus != "已退货" && o.FlowSheet.OrderStatus != "已取消"))
                .WhereIf(startDate.HasValue, o => o.CreationTime >= startDate.Value)
                .WhereIf(endDate.HasValue, o => o.CreationTime <= endDate.Value)
                .SumAsync(o => o.Price * o.Discount*o.OutNumber);
        }
        /// <summary>
        /// 获取剩余可开金额
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetUnitInvoiceRemainFee(int unitId, DateTime? startDate, DateTime? endDate)
        {
            var remainFee= (await GetUnitStoredFee(unitId, startDate, endDate)) - (await GetUnitInvoicedFee(unitId, startDate, endDate));
            if (remainFee < 0)
            {
                remainFee = 0;
            }
            return remainFee;
        }
        public override async Task<Dictionary<string,object>> GetPageSummary(IQueryable<Invoice> queryable,RequestPageDto requestPageDto)
        {
            var result = new Dictionary<string, object>();
            var searchKeys = requestPageDto.SearchKeyDic;
            if (searchKeys.ContainsKey("unitId"))
            {
                var unitId = int.Parse(searchKeys["unitId"]);
                DateTime? startDate=null,endDate = null;
                if (searchKeys.ContainsKey("startDate"))
                {
                    startDate = DateTime.Parse(searchKeys["startDate"]);
                }
                if (searchKeys.ContainsKey("endDate"))
                {
                    endDate = DateTime.Parse(searchKeys["endDate"]);
                }
                //如果查询中包含代理商，则需要查询出进货总额、已开总额
                var invoicedFee = await GetUnitInvoicedFee(unitId, startDate, endDate);
                var storedFee = await GetUnitStoredFee(unitId, startDate, endDate);
                result.Add("进货总额", storedFee.ToString("0.00"));
                result.Add("已开总额", invoicedFee.ToString("0.00"));
                result.Add("剩余可开金额", (storedFee-invoicedFee).ToString("0.00"));
            }
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

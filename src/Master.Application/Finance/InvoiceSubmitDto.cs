using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    public class InvoiceSubmitDto
    {
        public int UnitId { get; set; }
        public IEnumerable<InvoiceDto> Items { get; set; }
    }
    [AutoMap(typeof(Invoice))]
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public decimal Number { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public string TaxRate { get; set; }
        public string SellUnitName { get; set; }
        public string BuyUnitName { get; set; }
        public string BuyUnitTaxNumber { get; set; }
        public string BuyUnitAddress { get; set; }
        public string BuyUnitPhone { get; set; }
        public string BuyUnitBank { get; set; }
        public string BuyUnitBankAccount { get; set; }
        public string Remarks { get; set; }
    }
}

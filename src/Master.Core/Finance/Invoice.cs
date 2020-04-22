using Master.Entity;
using Master.Module.Attributes;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Finance
{
    [InterModule("发票管理",GenerateDefaultButtons =false)]
    public class Invoice: BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName = "状态", Sort = 0,ColumnType =Module.ColumnTypes.Select, DictionaryName = "Master.Finance.InvoiceStatus",Templet ="{{d.invoiceStatus_display}}")]
        public InvoiceStatus InvoiceStatus { get; set; }
        [InterColumn(ColumnName ="发票申请单编号",Sort =1)]
        public string SheetSN { get; set; }
        [InterColumn(ColumnName = "发票类别", Sort = 2)]
        public string Type { get; set; }
        [InterColumn(ColumnName = "开票内容", Sort = 3)]
        public string Content { get; set; }
        [InterColumn(ColumnName = "开票金额",ColumnType =Module.ColumnTypes.Number, Sort = 4)]
        [Column(TypeName ="decimal(20,2)")]
        public decimal Fee { get; set; }
        [InterColumn(ColumnName = "品牌单位名称", Sort = 5)]
        public string SellUnitName { get; set; }
        [InterColumn(ColumnName = "申请者", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 6)]
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        [InterColumn(ColumnName = "购货单位名称", Sort = 7)]
        public string BuyUnitName { get; set; }
        [InterColumn(ColumnName = "税务登记号", Sort = 8)]
        public string BuyUnitTaxNumber { get; set; }
        [InterColumn(ColumnName = "单位地址", Sort = 9)]
        public string BuyUnitAddress { get; set; }
        [InterColumn(ColumnName = "电话", Sort = 10)]
        public string BuyUnitPhone { get; set; }
        [InterColumn(ColumnName = "开户银行", Sort = 11)]
        public string BuyUnitBank { get; set; }
        [InterColumn(ColumnName = "开户行账号", Sort = 12)]
        public string BuyUnitBankAccount { get; set; }
        [InterColumn(ColumnName = "申请操作", Sort = 13)]
        public string OrderType { get; set; }
        [InterColumn(ColumnName = "备注", Sort = 14)]
        public string Remarks { get; set; }
    }
    public enum InvoiceStatus
    {
        待审核,
        已审核,
        已关闭
    }
}

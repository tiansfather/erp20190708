using Abp.Domain.Entities;
using Master.Entity;
using Master.Extension;
using Master.Module;
using Master.Module.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Units
{
    /// <summary>
    /// 往来单位
    /// </summary>
    [InterModule("往来单位",GenerateOperateColumn =true)]
    public class Unit : BaseFullEntityWithTenant, IPassivable
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        [InterColumn(ColumnName = "名称",VerifyRules ="required",Sort =0)]
        public virtual string UnitName { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [InterColumn(ColumnName = "简称", VerifyRules = "required", Sort = 1)]
        public virtual string BriefName { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        [InterColumn(ColumnName = "单位类型", ColumnType = ColumnTypes.Select,DefaultValue = "代理商", DictionaryName = "Master.Units.UnitNature", Sort = 2, Templet = "{{d.unitNature_display}}", EnableDataFilter = true)]
        public virtual UnitNature UnitNature { get; set; } = UnitNature.代理商;
        [InterColumn(ColumnName = "期初结余", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0.00", DefaultValue = "0", VerifyRules = "number|required", Sort = 3)]
        public decimal StartFee { get; set; }
        /// <summary>
        /// 当前余额
        /// </summary>
        [InterColumn(ColumnName = "当前结余", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0.00", IsShownInAdd = false, IsShownInEdit = false, Sort = 4,Templet = "{{(Math.round((parseFloat(d.startFee)+parseFloat(d.fee))*100)/100).toFixed(2)}}")]
        public decimal Fee { get; set; }
       
        [InterColumn(ColumnName = "联系地址", Sort = 5,IsShownInList =false)]
        public string Address { get; set; }
        [InterColumn(ColumnName = "邮编", Sort = 6, IsShownInList = false)]
        public string ZipCode { get; set; }
        [InterColumn(ColumnName = "联系人", Sort = 7, IsShownInList = false)]
        public string ContactPerson { get; set; }
        [InterColumn(ColumnName = "联系电话", Sort = 8, IsShownInList = false)]
        public string Phone { get; set; }      
        [InterColumn(ColumnName ="等级", DefaultValue ="1",ColumnType =ColumnTypes.Select,DictionaryName = "{\"1\":\"1\",\"2\":\"2\",\"3\":\"3\",\"4\":\"4\",\"5\":\"5\"}", ControlFormat ="select", Sort =9)]
        public int UnitRank { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [InterColumn(ColumnName = "状态", ColumnType = Module.ColumnTypes.Switch, DefaultValue = "true", Templet = "{{#if(d.isActive){}}<span class=\"layui-badge layui-bg-green\">有效</span>{{#}else{}}<span class=\"layui-badge layui-bg-gray\">无效</span>{{#}}}", Sort = 10, EnableDataFilter = true)]
        public bool IsActive { get; set; } = true;
        [InterColumn(ColumnName = "备注", Sort = 11, IsShownInList = false)]
        public override string Remarks { get; set; }
        [NotMapped]
        public UnitInvoice UnitInvoice
        {
            get
            {
                var invoice= this.GetData<UnitInvoice>("UnitInvoice");
                return invoice ?? new UnitInvoice();
            }
            set
            {
                this.SetData("UnitInvoice", value);
            }
        }
    }

    public enum UnitNature
    {
        代理商,
        供应商,
    }

    public class UnitInvoice
    {
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
    }

    public class UnitEntityMapConfiguration : EntityMappingConfiguration<Unit>
    {
        public override void Map(EntityTypeBuilder<Unit> b)
        {
            b.HasOne(p => p.DeleterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

            b.HasOne(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

            b.HasOne(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);

        }
    }
}

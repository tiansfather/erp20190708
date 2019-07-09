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
    [InterModule("往来单位")]
    public class Unit : BaseFullEntityWithTenant, IHaveAddress, IPassivable
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        [InterColumn(ColumnName = "名称",VerifyRules ="required",Sort =0)]
        public virtual string UnitName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [InterColumn(ColumnName = "编号", Sort = 1)]
        public virtual string UnitSN { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        [InterColumn(ColumnName = "性质", ColumnType = ColumnTypes.Select, DictionaryName = StaticDictionaryNames.UnitNature, Sort = 2, Templet = "{{d.unitNature==1?'客户':(d.unitNature==2?'供应商':'客户及供应商')}}")]
        public virtual UnitNature UnitNature { get; set; } = UnitNature.客户及供应商;
        /// <summary>
        /// 供应类别
        /// </summary>
        [InterColumn(ColumnName = "供应类别", ColumnType = ColumnTypes.MultiSelect, DictionaryName = StaticDictionaryNames.SupplierType, Sort = 3)]
        public virtual string SupplierType { get; set; } 
        public virtual BaseTree UnitType { get; set; }
        [InterColumn(ColumnName ="分类",ColumnType =ColumnTypes.Text,Renderer ="lay-unittypechoose",DisplayPath ="UnitType.DisplayName",Templet = "{{d.unitTypeId_display||''}}", Sort = 3)]
        public int? UnitTypeId { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [InterColumn(ColumnName = "简称", VerifyRules = "required", Sort = 4)]
        public virtual string BriefName { get; set; }
        [InterColumn(ColumnName = "国家", Sort = 15)]
        public string Country { get; set; }
        [InterColumn(ColumnName = "省份", Sort = 16)]
        public string Province { get; set; }
        [InterColumn(ColumnName = "城市", Sort = 17)]
        public string District { get; set; }
        [InterColumn(ColumnName = "地址", Sort = 18)]
        public string Address { get; set; }
        [InterColumn(ColumnName = "电话", Sort = 19)]
        public string Phone { get; set; }

        [InterColumn(ColumnName = "备注", Sort = 30)]
        public override string Remarks { get; set; }
        /// <summary>
        /// 应收应付金额,正数为应付,负数为应收
        /// </summary>
        public decimal Fee { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum UnitNature
    {
        客户=1,
        供应商=2,
        客户及供应商=3
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

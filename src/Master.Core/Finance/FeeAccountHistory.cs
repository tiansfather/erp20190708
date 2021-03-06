﻿using Master.Entity;
using Master.Module.Attributes;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Finance
{
    /// <summary>
    /// 账号金额变动明细
    /// </summary>
    [InterModule("资金往来明细",GenerateDefaultButtons =false)]
    public class FeeAccountHistory : BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName ="资金账户",DisplayPath="FeeAccount.Name",Templet = "{{d.feeAccountId_display||'/'}}", Sort =1, EnableDataFilter = true)]
        public int FeeAccountId { get; set; }
        public virtual FeeAccount FeeAccount { get; set; }
        [InterColumn(ColumnName = "往来单位", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 2, EnableDataFilter = true)]
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// 变化的金额,正数为应付增加，负数为应付减少
        /// </summary>
        [InterColumn(ColumnName ="发生金额",ColumnType =Module.ColumnTypes.Number,DisplayFormat ="N2", Sort = 3)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Fee { get; set; }
        [InterColumn(ColumnName ="流向",ColumnType =Module.ColumnTypes.Select,DictionaryName ="Master.Finance.FeeDirection",Templet ="{{d.feeDirection_display}}", Sort = 4, EnableDataFilter = true)]
        public FeeDirection FeeDirection { get; set; }
        [NotMapped]
        [InterColumn(ColumnName = "对方单位名称", ValuePath ="Property", Sort = 5, EnableDataFilter = true)]
        public string RelCompanyName { get; set; }
        [InterColumn(ColumnName = "账户余额", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "N2", Sort = 6)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal RemainFee { get; set; }
        [InterColumn(ColumnName ="发生时间",ColumnType =Module.ColumnTypes.DateTime,DisplayFormat ="yyyy-MM-dd HH:mm", Sort = 7)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName ="发生环节", ValuePath = "FlowSheet.FlowInstance.FlowForm.FormName", DisplayPath = "FlowSheet.FlowInstance.FlowForm.FormName",Templet ="{{d.formName_display}}", Sort = 8, EnableDataFilter = true)]
        [NotMapped]
        public string FormName => FlowSheet.FlowInstance.FlowForm.FormName;
        [InterColumn(ColumnName = "摘要", Sort = 9)]
        public string ChangeType { get; set; }
        /// <summary>
        /// 关联单据
        /// </summary>
        [InterColumn(ColumnName = "发生环节单据编号", Templet = "<a dataid=\"{{d.flowSheetId}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.flowSheetId_display}}</a>", DisplayPath = "FlowSheet.SheetSN", Sort = 10)]
        public int? FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        [InterColumn(ColumnName = "备注",DisplayPath = "FlowSheet.Remarks", Sort = 11, Templet = "{{d.remarks_display||''}}")]
        public override string Remarks { get; set; }
    }

    public enum FeeDirection
    {
        进,
        出
    }
}

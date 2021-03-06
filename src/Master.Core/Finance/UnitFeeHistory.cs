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
    /// 往来单位费用变动明细
    /// </summary>
    [InterModule("往来单位明细", GenerateDefaultButtons = false)]
    public class UnitFeeHistory : BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName = "往来单位", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display}}", Sort = 1, EnableDataFilter = true)]
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        [InterColumn(ColumnName = "发生时间", ColumnType = Module.ColumnTypes.DateTime, DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 2)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        /// <summary>
        /// 变化的金额,正数为应付增加，负数为应付减少
        /// </summary>
        [InterColumn(ColumnName = "发生金额", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "N2", Sort = 3)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Fee { get; set; }
        [InterColumn(ColumnName = "发生后结余", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "N2", Sort = 4)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal RemainFee { get; set; }
        [InterColumn(ColumnName = "发生环节", ValuePath = "FlowSheet.FlowInstance.FlowForm.FormName", DisplayPath = "FlowSheet.FlowInstance.FlowForm.FormName", Templet = "{{d.formName_display}}", Sort = 5, EnableDataFilter = true)]
        [NotMapped]
        public string FormName => FlowSheet.FlowInstance.FlowForm.FormName;
        [InterColumn(ColumnName = "摘要", Sort = 6)]
        public string ChangeType { get; set; }
        /// <summary>
        /// 关联单据
        /// </summary>
        [InterColumn(ColumnName = "发生环节单据编号", Templet = "<a dataid=\"{{d.flowSheetId}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.flowSheetId_display}}</a>", DisplayPath = "FlowSheet.SheetSN", Sort = 7)]
        public int? FlowSheetId { get; set; }
        public virtual FlowSheet FlowSheet { get; set; }
        [InterColumn(ColumnName = "对方单位名称", Sort = 8, EnableDataFilter = true)]
        public string RelCompanyName { get; set; }
    }
}

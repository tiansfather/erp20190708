﻿using Master.Module.Attributes;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Modules
{
    [InterModule("实物订单", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class SDRSheet : FlowSheet
    {
        [InterColumn(ColumnName = "订单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "下单代理", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 2)]
        public override int? UnitId { get => base.UnitId; set => base.UnitId = value; }
        [InterColumn(ColumnName = "下单时间", ColumnType = Module.ColumnTypes.DateTime,Sort = 3)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display||'/'}}", Sort = 4)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName ="下单操作",ValuePath ="Property",Sort =5)]
        [NotMapped]
        public virtual decimal OrderType{get;set;}
        [InterColumn(ColumnName = "当前状态", Sort =6,Templet ="{{getStatusHtml(d.orderStatus)}}")]
        public override string OrderStatus { get; set; }
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        public override SheetNature SheetNature { get => base.SheetNature; set => base.SheetNature = value; }
        [InterColumn(ColumnName = "对应进货单", ValuePath = "Property", Sort = 100, EnableDataFilter = false,Templet = "{{#if(d.prhSheetId){}}<a dataid=\"{{d.prhSheetId}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.prhSheetSN}}</a>{{#}else{}}<font color=\"red\">无</font>{{#}}}")]
        [NotMapped]
        public virtual string PRHSheetSN { get; set; }
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }
    }
    [InterModule("实物订单审核", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class SDRVerify : SDRSheet
    {
        [InterColumn(ColumnName = "订单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
    }
    [InterModule("实物订单发货", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class SDRSend : FlowSheet
    {
        [InterColumn(ColumnName = "订单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "下单代理", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display}}", Sort = 2)]
        public override int? UnitId { get => base.UnitId; set => base.UnitId = value; }
        [InterColumn(ColumnName = "下单时间",  ColumnType = Module.ColumnTypes.DateTime,Sort = 3)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display||'/'}}", Sort = 4)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName = "下单操作", ValuePath = "Property", Sort = 5)]
        [NotMapped]
        public virtual decimal OrderType { get; set; }
        [InterColumn(ColumnName = "当前状态", Sort = 6, Templet = "{{getStatusHtml(d.orderStatus)}}")]
        public override string OrderStatus { get; set; }
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        public override SheetNature SheetNature { get => base.SheetNature; set => base.SheetNature = value; }
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }
    }
    [InterModule("实物订单退货", BaseType = typeof(FlowSheet), GenerateDefaultButtons = false)]
    public class SDRBack : FlowSheet
    {
        [InterColumn(ColumnName = "订单编号", Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>", Sort = 1)]
        public override string SheetSN { get => base.SheetSN; set => base.SheetSN = value; }
        [InterColumn(ColumnName = "下单代理", DisplayPath = "Unit.UnitName", Templet = "{{d.unitId_display||'/'}}", Sort = 2, EnableDataFilter = true)]
        public override int? UnitId { get => base.UnitId; set => base.UnitId = value; }
        [InterColumn(ColumnName = "下单时间", ColumnType = Module.ColumnTypes.DateTime, Sort = 3)]
        public override DateTime CreationTime { get => base.CreationTime; set => base.CreationTime = value; }
        [InterColumn(ColumnName = "经办人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display||'/'}}", Sort = 4, EnableDataFilter = true)]
        public override long? CreatorUserId { get; set; }
        [InterColumn(ColumnName = "下单操作", ValuePath = "Property", Sort = 5, EnableDataFilter = true)]
        [NotMapped]
        public virtual decimal OrderType { get; set; }
        [InterColumn(ColumnName = "当前状态", Sort = 6, EnableDataFilter = true, Templet = "{{getStatusHtml(d.orderStatus)}}")]
        public override string OrderStatus { get; set; }
        public override DateTime SheetDate { get => base.SheetDate; set => base.SheetDate = value; }
        public override SheetNature SheetNature { get => base.SheetNature; set => base.SheetNature = value; }        
        [InterColumn(ColumnName = "创建时间", ColumnType = Module.ColumnTypes.DateTime, ValuePath = "CreationTime", DisplayFormat = "yyyy-MM-dd HH:mm", Sort = 999)]
        [NotMapped]
        public DateTime CreationTime2 { get; set; }
    }
}

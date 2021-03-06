﻿using Master.Authentication;
using Master.Entity;
using Master.Module.Attributes;
using Master.Projects;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 单据
    /// </summary>
    [Table("FlowSheet")]
    public class FlowSheet : BaseFullEntityWithTenant, IHaveStatus
    {
        public int? FlowInstanceId { get; set; }
        public virtual FlowInstance FlowInstance { get; set; }
        public string FormKey { get; set; }
        /// <summary>
        /// 往来单位Id
        /// </summary>
        [InterColumn(ColumnName = "往来单位", ValuePath = "Unit.UnitName")]
        public virtual int? UnitId { get; set; }
        /// <summary>
        /// 往来单位
        /// </summary>
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        [InterColumn(ColumnName = "单号",Sort =-1)]
        public virtual string BusinessSN { get; set; }
        /// <summary>
        /// 单据名称,用于标记单据如cgrk
        /// </summary>
        [InterColumn(ColumnName = "单据名称")]
        public virtual string SheetName { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [InterColumn(ColumnName = "单据编号",Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>")]
        public virtual string SheetSN { get; set; }
        /// <summary>
        /// 单据日期
        /// </summary>
        [InterColumn(ColumnName = "单据日期")]
        public virtual DateTime SheetDate { get; set; }
        /// <summary>
        /// 业务类型:此项用于标记分类单据
        /// </summary>
        public virtual string BusinessType { get; set; }
        
        /// <summary>
        /// 经办人Id
        /// </summary>
        [InterColumn(ColumnName = "经办人", ValuePath = "Handler.Name")]
        public virtual long? HandlerId { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public virtual User Handler { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [InterColumn(ColumnName = "项目编号", ValuePath = "Project.ProjectSN")]
        public virtual int? ProjectId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public virtual Project Project { get; set; }
        
        
        /// <summary>
        /// 单据状态
        /// </summary>
        public virtual SheetStatus SheetStatus { get; set; } = SheetStatus.进行中;
        public virtual string Status { get;set; }
        public virtual ICollection<FlowSheetContent> SheetContents { get; set; } = new List<FlowSheetContent>();

        /// <summary>
        /// 单据附件
        /// </summary>
        [NotMapped]
        public IEnumerable<UploadFileInfo> Files
        {
            get
            {
                var result=this.GetPropertyValue<IEnumerable<UploadFileInfo>>("Files");
                if (result == null)
                {
                    result = new List<UploadFileInfo>();
                }
                return result;
            }
            set
            {
                this.SetPropertyValue("Files", value);
            }
        }

    }

    /// <summary>
    /// 单据中的列表内容
    /// </summary>
    [Table("FlowSheetContent")]
    public class FlowSheetContent: BaseFullEntityWithTenant, IHaveStatus
    {
        public int SheetId { get; set; }
        public virtual FlowSheet Sheet { get; set; }
        /// <summary>
        /// 关联单据明细Id
        /// </summary>
        public int? SheetContentId { get; set; }
        public virtual FlowSheetContent RelativeSheetContent { get; set; }
        public string Status { get; set; }
    }

    public enum SheetStatus
    {
        草稿=-1,
        进行中=1,
        已结束=2
    }
}

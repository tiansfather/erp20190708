using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Authentication;
using Master.Entity;
using Master.Module.Attributes;
using Master.Projects;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Master.WorkFlow
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
        public virtual int? UnitId { get; set; }
        /// <summary>
        /// 往来单位
        /// </summary>
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public virtual string BusinessSN { get; set; }
        /// <summary>
        /// 单据名称,用于标记单据如cgrk
        /// </summary>
        public virtual string SheetName { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [InterColumn(ColumnName = "单据编号",Templet = "<a dataid=\"{{d.id}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.sheetSN}}</a>",Sort =1)]
        public virtual string SheetSN { get; set; }
        /// <summary>
        /// 单据日期
        /// </summary>
        [InterColumn(ColumnName = "单据日期",Sort =10)]
        public virtual DateTime SheetDate { get; set; }
        /// <summary>
        /// 业务类型:此项用于标记分类单据
        /// </summary>
        public virtual string BusinessType { get; set; }

        [InterColumn(ColumnName = "制单人", DisplayPath = "CreatorUser.Name", Templet = "{{d.creatorUserId_display}}", Sort = 20)]
        public override long? CreatorUserId { get; set; }

        /// <summary>
        /// 经办人Id
        /// </summary>
        public virtual long? HandlerId { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public virtual User Handler { get; set; } 
        [InterColumn(ColumnName ="性质",ColumnType =Module.ColumnTypes.Select,DictionaryName ="Master.WorkFlow.SheetNature",Templet ="{{d.sheetNature_display}}",Sort =30)]
        public SheetNature SheetNature { get; set; }
        [InterColumn(ColumnName = "备注",Sort =40)]
        public override string Remarks { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public virtual SheetStatus SheetStatus { get; set; } = SheetStatus.进行中;
        public virtual string Status { get;set; }
        public virtual ICollection<FlowSheetContent> SheetContents { get; set; } = new List<FlowSheetContent>();
        /// <summary>
        /// 关联单据
        /// </summary>
        public virtual int? RelSheetId { get; set; }
        public virtual FlowSheet RelSheet { get; set; }
        /// <summary>
        /// 冲红原因
        /// </summary>
        public virtual string RevertReason { get; set; }

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

    public enum SheetNature
    {
        正单,
        冲红,
        被冲红
    }

    /// <summary>
    /// 单据事件
    /// </summary>
    public class FlowSheetEventHandler :
        IEventHandler<EntityCreatedEventData<FlowSheet>>,
        ITransientDependency
    {
        private static object _obj = new object();
        public FlowSheetManager FlowSheetManager { get; set; }
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<FlowSheet> eventData)
        {
            var entity = eventData.Entity;
            if (string.IsNullOrEmpty(entity.SheetSN))
            {
                entity.SheetSN = FlowSheetManager.GenerateSheetSN(entity.FormKey);
                FlowSheetManager.UpdateAsync(entity).GetAwaiter().GetResult();
            }
            ////生成单据编号20190308001
            //lock (_obj)
            //{
                
            //    //当天数量
            //    var todayNum = FlowSheetManager.GetAll().Where(o => o.FormKey == entity.FormKey && o.CreationTime.Year == DateTime.Now.Year && o.CreationTime.Month == DateTime.Now.Month && o.CreationTime.Day == DateTime.Now.Day).Count();
            //    var newNum = todayNum + 1;

            //    entity.SheetSN = entity.FormKey + DateTime.Now.ToString("yyyyMMdd") + newNum.ToString().PadLeft(3, '0');

            //    FlowSheetManager.UpdateAsync(entity).GetAwaiter().GetResult();
            //}
        }
    }
}

using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.WorkFlow.Events
{
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
            //生成单据编号20190308001
            lock (_obj)
            {
                var entity = eventData.Entity;
                //当天数量
                var todayNum = FlowSheetManager.GetAll().Where(o => o.FormKey == entity.FormKey && o.CreationTime.Year == DateTime.Now.Year && o.CreationTime.Month == DateTime.Now.Month && o.CreationTime.Day == DateTime.Now.Day).Count();
                var newNum = todayNum + 1;

                entity.SheetSN = entity.FormKey + DateTime.Now.ToString("yyyyMMdd") + newNum.ToString().PadLeft(3, '0');

                FlowSheetManager.UpdateAsync(entity).GetAwaiter().GetResult();
            }
        }
    }
}

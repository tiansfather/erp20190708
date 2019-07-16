using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    public class FlowInstanceManager : ModuleServiceBase<FlowInstance, int>, IEventHandler<EntityCreatedEventData<FlowInstance>>
    {
        //public IFlowHandlerFinder FlowHandlerFinder { get; set; }
        //public FlowFormManager FlowFormManager { get; set; }
        //public FlowSheetManager FlowSheetManager { get; set; }
        public virtual async Task CreateInstance(FlowInstance flowInstance)
        {
            await InsertAsync(flowInstance);
            await CurrentUnitOfWork.SaveChangesAsync();
            var form = await Resolve<FlowFormManager>().GetByIdFromCacheAsync(flowInstance.FlowFormId);

            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(form.FormKey);

            await flowHandler.CreateSheet(flowInstance, form);
        }
        /// <summary>
        /// 当一个流程审核通过后调用
        /// </summary>
        /// <param name="flowInstance"></param>
        /// <returns></returns>
        public virtual async Task FinishInstance(FlowInstance flowInstance)
        {
            var form = await Resolve<FlowFormManager>().GetByIdFromCacheAsync(flowInstance.FlowFormId);

            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(form.FormKey);

            var flowSheet = await Resolve<FlowSheetManager>().GetByInstanceId(flowInstance.Id);

            await flowHandler.Handle(flowSheet);
        }
        /// <summary>
        /// 流程建立后自动建立表单
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<FlowInstance> eventData)
        {
            //var flowInstance = eventData.Entity;
            //var form = Resolve<FlowFormManager>().GetByIdFromCacheAsync(flowInstance.FlowFormId).Result;

            //var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(form.FormKey);

            //flowHandler.CreateSheet(flowInstance, form).GetAwaiter().GetResult();
        }
    }
}

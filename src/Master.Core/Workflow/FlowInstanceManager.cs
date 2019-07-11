using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    public class FlowInstanceManager : ModuleServiceBase<FlowInstance, int>
    {
        //public IFlowHandlerFinder FlowHandlerFinder { get; set; }
        //public FlowFormManager FlowFormManager { get; set; }
        //public FlowSheetManager FlowSheetManager { get; set; }
        /// <summary>
        /// 当一个流程审核通过后调用
        /// </summary>
        /// <param name="flowInstance"></param>
        /// <returns></returns>
        public virtual async Task FinishInstance(FlowInstance flowInstance)
        {
            var form = await Resolve<FlowFormManager>().GetByIdFromCacheAsync(flowInstance.FlowFormId);

            var flowHandler = Resolve<IFlowHandlerFinder>().FindFlowHandler(form.FormKey);

            await flowHandler.Handle(flowInstance,form);
        }

    }
}

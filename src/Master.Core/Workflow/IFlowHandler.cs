using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    public interface IFlowHandler:ITransientDependency
    {
        /// <summary>
        /// 流程的处理
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="flowForm"></param>
        /// <returns></returns>
        Task Handle(FlowInstance instance,FlowForm flowForm);
    }
}

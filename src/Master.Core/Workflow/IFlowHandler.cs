using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    public interface IFlowHandler:ITransientDependency
    {
        
        Task Handle(FlowSheet flowSheet);

        Task<FlowSheet> CreateSheet(FlowInstance instance, FlowForm flowForm);

        Task CreateRevertSheet(FlowSheet flowSheet, string revertReason);
        Task HandleRevert(FlowSheet flowSheet);
    }
}

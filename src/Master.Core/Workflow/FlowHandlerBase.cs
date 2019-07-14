using Abp.Dependency;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    /// <summary>
    /// 表单处理基类
    /// </summary>
    public abstract class FlowHandlerBase : IFlowHandler
    {
        public abstract Task Handle(FlowInstance instance, FlowForm flowForm);

        public abstract Task HandleRevert(FlowInstance flowInstance, FlowSheet flowSheet);

        protected T Resolve<T>()
        {
            using (var wrapper=IocManager.Instance.ResolveAsDisposable<T>())
            {
                return wrapper.Object;
            }
        }
    }
}

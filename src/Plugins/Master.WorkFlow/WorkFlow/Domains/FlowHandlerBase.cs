using Abp.Dependency;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 表单处理基类
    /// </summary>
    public abstract class FlowHandlerBase : IFlowHandler
    {
        public abstract Task Handle(FlowInstance instance, FlowForm flowForm);

        protected T Resolve<T>()
        {
            using (var wrapper=IocManager.Instance.ResolveAsDisposable<T>())
            {
                return wrapper.Object;
            }
        }
    }
}

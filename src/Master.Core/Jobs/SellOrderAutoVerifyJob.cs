using Abp.Dependency;
using Abp.Quartz;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Jobs
{
    public class SellOrderAutoVerifyJob : JobBase, ISingletonDependency
    {
        public override async Task Execute(IJobExecutionContext context)
        {
            
        }
    }
}

using Abp.Configuration.Startup;
using Master.WorkFlow.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WorkFlowConfiguration
    {
        public List<FlowForm> DefaultForms { get; private set; } = new List<FlowForm>();
    }

    public static class WorkFlowConfigurationExtension
    {
        public static WorkFlowConfiguration WorkFlow(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<WorkFlowConfiguration>();
        }
    }
}

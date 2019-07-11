using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WorkFlowFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}

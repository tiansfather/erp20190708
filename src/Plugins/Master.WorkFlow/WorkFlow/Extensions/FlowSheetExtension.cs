using System;
using System.Collections.Generic;
using System.Text;
using Master.Entity;
using Master.WorkFlow;
using Master.WorkFlow.Domains;

namespace Master.WorkFlow.Extensions
{
    public static class FlowSheetExtension
    {
        public static bool IsAvailableForTenant(this FlowSheet flowSheet,int tenantId)
        {            
            return flowSheet.TenantId == tenantId || flowSheet.Unit?.GetPropertyValue<int?>("TenantId") == tenantId;
        }
    }
}

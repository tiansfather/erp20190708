using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Master.Domain;

namespace Master.WorkFlow.Domains
{
    public interface IFlowSheetManager:IData<FlowSheet,int>
    {
        /// <summary>
        /// 获取单据模板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> GetSheetTemplateByName(string name);
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using Master.Domain;

namespace Master.WorkFlow
{
    public interface IFlowHandlerFinder:ITransientDependency
    {
        /// <summary>
        /// 寻找单据的处理类
        /// </summary>
        /// <param name="formKey">表单标识</param>
        /// <returns></returns>
        IFlowHandler FindFlowHandler(string formKey);
    }

    

    //public class Default2SheetHandlerFinder : ISheetHandlerFinder, ITransientDependency
    //{
    //    //...
    //    public ISheetHandler FindSheetHandler(Sheet sheet)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

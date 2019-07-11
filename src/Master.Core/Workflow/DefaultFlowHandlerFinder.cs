using Abp.Dependency;
using Abp.Reflection;
using Abp.UI;
using Master.Domain;
using Master.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.WorkFlow
{
    public class DefaultFlowHandlerFinder :  IFlowHandlerFinder, ITransientDependency
    {
        public ITypeFinder TypeFinder { get; set; }
        public FlowFormManager FlowFormManager { get; set; }
        /// <summary>
        /// 寻找单据的处理类
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        //public IFlowHandler[] FindFlowHandler(FlowSheet sheet)
        //{

        //    //默认按照sheetname大写去找
        //    //如cgrk单,其处理类为CGRKHandler;
        //    var handlerTypeName = $"{sheet.SheetName?.ToUpper()}Handler";
        //    var type = TypeFinder.Find(o => o.Name == handlerTypeName).FirstOrDefault();
        //    if (type == null)
        //    {
        //        throw new UserFriendlyException($"SheetHandler {handlerTypeName} not found");
        //    }

        //    using(var handlerWrapper = IocManager.Instance.ResolveAsDisposable(type))
        //    {
        //        return handlerWrapper.Object as IFlowSheetHandler;
        //    }
        //}

        /// <summary>
        /// 根据表单标识寻找表单处理类
        /// </summary>
        /// <param name="formKey"></param>
        /// <returns></returns>
        public IFlowHandler FindFlowHandler(string formKey)
        {
            IFlowHandler handler = null;
            //先去寻找自定义表单处理程序
            var flowForm = FlowFormManager.GetAll().Where(o => o.FormKey == formKey).FirstOrDefault();

            //如果表单已有处理程序
            if (!string.IsNullOrWhiteSpace(flowForm.FormHandler))
            {
                var type = ScriptRunner.CompileType(flowForm.FormHandler);
                handler = Activator.CreateInstance(type) as IFlowHandler;
            }
            else
            {
                //按照约定去程序集中寻找处理程序类
                handler = FindFlowHandlerFromConvention(formKey);
            }
            
            if (handler == null)
            {
                throw new UserFriendlyException($"{formKey}FlowHandler not found");
            }

            return handler;
        }

        private IFlowHandler FindFlowHandlerFromConvention(string formKey)
        {
            var handlerTypeName = $"{formKey}FlowHandler";
            var type = TypeFinder.Find(o => o.Name == handlerTypeName).FirstOrDefault();
            //if (type == null)
            //{
            //    throw new UserFriendlyException($"{formKey}FlowHandler not found");
            //}
            if (type != null)
            {
                using (var handlerWrapper = IocManager.Instance.ResolveAsDisposable(type))
                {
                    return handlerWrapper.Object as IFlowHandler;
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}

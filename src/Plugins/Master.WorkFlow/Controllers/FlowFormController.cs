﻿using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration.Startup;
using Master.Authentication;
using Master.Configuration;
using Master.WorkFlow.Domains;
using Master.WorkFlow.Extensions;
using Master.WorkFlow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class FlowFormController : MasterModuleControllerBase
    {
        public FlowFormManager FormManager { get; set; }
        public FlowSchemeManager FlowSchemeManager { get; set; }
        public FlowInstanceManager FlowInstanceManager { get; set; }
        public FlowSheetManager FlowSheetManager { get; set; }
        public UserManager UserManager { get; set; }
        public FlowFormManager FlowFormManager { get; set; }
        public IAbpStartupConfiguration Configuration { get; set; }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单设计
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IActionResult> Design(int data)
        {
            var form = await FormManager.GetByIdFromCacheAsync(data);
            switch (form.FormType)
            {
                case FormType.Html:
                    return View("Html");
                case FormType.Designer:
                    return View("Designer");
                case FormType.Spread:
                    return View("Spread");
            }
            return View();
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IActionResult> Handler(int data)
        {
            return View();
        }
        /// <summary>
        /// 表单录入
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IActionResult> Input(int data)
        {
            var form = await FormManager.GetByIdFromCacheAsync(data);
            var defaultForms= Configuration.Modules.WorkFlow().DefaultForms;
            var defaultForm = defaultForms.FirstOrDefault(o => o.FormKey == form.FormKey);
            switch (form.FormType)
            {
                case FormType.Html:
                    ViewBag.FormContent = string.IsNullOrEmpty(form.FormContent)?defaultForm?.FormContent:form.FormContent;
                    return View("HtmlInput");
                case FormType.Designer:
                    return View("DesignerInput");
                case FormType.Spread:
                    return View("SpreadInput");
            }
            return View();
        }

        /// <summary>
        /// 通过表单标志录入表单
        /// </summary>
        /// <param name="formKey"></param>
        /// <param name="data">表单额外参数</param>
        /// <returns></returns>
        public async Task<IActionResult> InputByFormKey(string formKey,string data)
        {
            //先建立默认表单
            await Resolve<FlowFormAppService>().InitDefaultForm(AbpSession.TenantId);
            var form = await FormManager.GetByKey(formKey);
            if (form == null)
            {                
                return Error("未找到对应表单");
            }
            //寻找表单第一个流程定义
            var flowScheme = await FlowSchemeManager.GetAll().Where(o => o.FlowFormId == form.Id).FirstOrDefaultAsync();
            return Redirect($"/FlowForm/Input?data={form.Id}&params={data}&flowSchemeId={(flowScheme == null ? "" : flowScheme.Id.ToString())}");
        }
        /// <summary>
        /// 流程实例的表单查看
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IActionResult> InstanceView(int data)
        {
            var instance = await FlowInstanceManager.GetAll().IgnoreQueryFilters().Where(o => o.Id == data).FirstOrDefaultAsync();
            if (instance == null)
            {
                return Error(L("此单据已不存在"));
            }
            
            switch (instance.FormType)
            {
                case FormType.Html:
                    ViewBag.FormContent = instance.FormContent;
                    ViewBag.FormData = instance.FormData;
                    return View("HtmlView");
                case FormType.Designer:
                    return View("DesignerView");
                case FormType.Spread:
                    return View("SpreadView");
            }
            return View();
        }

        /// <summary>
        /// 表单的缓存 页查看
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ActionResult> FormCacheView(string code)
        { 
            var formObj = await GetFormCache(code);
            ViewBag.FormContent = formObj["formContent"];
            ViewBag.FormData = formObj["formData"];
            return View("HtmlView");
        }
        /// <summary>
        /// 表单的缓存获取 并设置用户为当前扫码打开页面人
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<JObject> GetFormCache(string code)
        {
            var userPerson = await UserManager.GetByIdAsync((long)AbpSession.UserId);
            var instance = CacheManager.GetCache("FormCache").GetOrDefault(code);
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(instance.ToString());
            formObj["formData"]["layoutData"]["usePerson"] = userPerson.Name;

            CacheManager.GetCache("FormCache").Set(code, formObj);
            // var flowForm = FlowFormManager.GetAll().Where(o => o.Id == Convert.ToInt32(formObj["formId"])).FirstOrDefault();
            var flowForm = await FlowFormManager.GetByIdAsync(Convert.ToInt32(formObj["formId"]));
            formObj["formContent"] = flowForm.FormContent;
            return formObj;
        }

        /// <summary>
        /// 表单的设置缓存
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string SetFormCache(string formData,string formId)
        {
            var formDataJson = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(formData);
            var jsonData = new JObject();
            jsonData["formData"] = formDataJson;
            jsonData["formId"] = formId;
       //     { formData = formDataJson, formId }
            var code =   Guid.NewGuid().ToString();
            CacheManager.GetCache("FormCache").Set(code, jsonData);
            return code;
        }
        /// <summary>
        /// 表单的缓存中的领取人名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetFormCachePerson(string code)
        {
            string Name = null;
            try
            {
                var instance = CacheManager.GetCache("FormCache").GetOrDefault(code);
                var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(instance.ToString());
                Name = formObj["formData"]["layoutData"]["usePerson"].ToString();
            }
            catch (Exception ex) {
                return null;
            }
            return Name;
        }
        public async Task<IActionResult> InstanceRepost(int data)
        {
            var instance = await FlowInstanceManager.GetByIdFromCacheAsync(data);
            switch (instance.FormType)
            {
                case FormType.Html:
                    ViewBag.FormContent = instance.FormContent;
                    ViewBag.FormData = instance.FormData;
                    return View("HtmlRepost");
                case FormType.Designer:
                    return View("DesignerRepost");
                case FormType.Spread:
                    return View("SpreadRepost");
            }
            return View();
        }
    }
}

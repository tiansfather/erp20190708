using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Authentication.External;
using Master.Configuration;
using Master.Controllers;
using Master.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Master.Entity;
using Master.MultiTenancy;
using Master.Menu;
using Abp.Reflection;
using Abp.Application.Features;
using Master.Models.TokenAuth;
using Master.Projects;
using Master.EntityFrameworkCore;
using Master.Domain;
using Abp.Configuration.Startup;
using Microsoft.Extensions.Options;
using Common.Configuration;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Master.Web.Controllers
{    
    public interface ITestJob
    {
        int DoJob();
    }

    public class HomeController : MasterControllerBase
    {
        private ISessionAppService _sessionAppService;
        private readonly UserManager _userManager;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;


        

        public IRepository<BaseTree,int> BaseTreeRepository { get; set; }
        public IRepository<Tenant,int> TenantRepository { get; set; }
        public ITypeFinder TypeFinder { get; set; }
        public TokenAuthController TokenAuthController { get; set; }
        public ProjectManager ProjectManager { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }
        public IAbpStartupConfiguration AbpStartupConfiguration { get; set; }
        public HomeController(
            ISessionAppService sessionAppService,
            IHostingEnvironment env, 
            UserManager userManager,
            IRepository<Setting> settingRepository, 
            IExternalAuthConfiguration externalAuthConfiguration)
        {
            _userManager = userManager;
            _settingRepository = settingRepository;
            _sessionAppService = sessionAppService;
            _appConfiguration = env.GetAppConfiguration();
            _externalAuthConfiguration = externalAuthConfiguration;
        }
        
        [AbpMvcAuthorize]
        public async Task<ActionResult> Index()
        {
            var user = AbpSession.ToUserIdentifier();
            Session.Dto.LoginInformationDto loginInfo;
            try
            {                
                loginInfo = await _sessionAppService.GetCurrentLoginInformations();
                if (AbpSession.MultiTenancySide==Abp.MultiTenancy.MultiTenancySides.Tenant && !loginInfo.Tenant.IsActive)
                {
                    throw new Exception("账套未激活");
                }
                
            }
            catch
            {
                Response.Cookies.Delete("token");
                return Redirect("/Account/Login");
            }
            //默认首页
            if (loginInfo.User.HomeUrl.IsNullOrEmpty())
            {
                loginInfo.User.HomeUrl = "Home/Default";
            }
            return View(loginInfo);
        }
        public IActionResult Default()
        {
            var viewName= AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Tenant ? "Default" : "HostDefault";
            return View(viewName);
        }
        /// <summary>
        /// 展示视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IActionResult Show(string name)
        {
            return View(name);
        }

        /// <summary>
        /// 企业信息
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult Tenant()
        {
            return View();
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult UserInfo()
        {
            var providers = _externalAuthConfiguration.Providers;
            //bool.TryParse(_appConfiguration["Authentication:Wechat:IsEnabled"], out var enableWeChatAuthentication);
            //bool.TryParse(_appConfiguration["Authentication:MiniProgram:IsEnabled"], out var enableMiniProgramAuthentication);
            //ViewBag.EnableWeChatAuthentication = enableWeChatAuthentication;
            //ViewBag.EnableMiniProgramAuthentication = enableMiniProgramAuthentication;
            ViewBag.ExternalAuthProviders = providers;
            return View();
        }
        

        public IActionResult Test()
        {

            return View();
            //return Content(AbpStartupConfiguration.Modules.WebCore().BaseUrl);
            //MySqlJsonFunctions
            //return Content(_userManager.GetAll().Where(o =>o.Status.Contains("o")).Count().ToString());
            //var obj=DynamicQuery.Single<int?>($"select sum(datediff( case when enddate is null then NOW() else enddate end,requiredate)) from processtask where requiredate is not null and enddate is not null and partid in (select id from part where projectid=1 and isdeleted=0) and isdeleted=0");

            //return Content(obj?.ToString());

            //var c=ProjectManager.GetAll().Where(o=>!string.IsNullOrEmpty(MasterDbContext.GetJsonValueString(o.Property, "$.ProjectCharger")))
            //    .GroupBy(o => MasterDbContext.GetJsonValueString(o.Property, "$.ProjectCharger")).ToList();
            
            //return Content(c.Count.ToString());
            //var user = _userManager.FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo("Wechat", "obsk_v497NScxPzY_mr9zh4ynRSs", "")).Result;
            //if (user != null)
            //{
            //    //生成token返回给客户端
            //    var authenticateResult = TokenAuthController.ExternalAuthenticate(new ExternalAuthenticateModel() { AuthProvider = "Wechat", ProviderKey = "obsk_v497NScxPzY_mr9zh4ynRSs" }).Result;
            //    HttpContext.Response.Cookies.Append("token", authenticateResult.EncryptedAccessToken,new Microsoft.AspNetCore.Http.CookieOptions());
            //}
            //HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            //// set browser width
            //htmlToPdfConverter.BrowserWidth = 1024;



            //// set PDF page margins
            //htmlToPdfConverter.Document.Margins = new PdfMargins(5);


            //htmlToPdfConverter.ConvertUrlToFile("http://localhost:62116/Home/Show?name=../MES/SheetView&taskid=648", @"c:\2.pdf");
            //return Content(FeatureChecker.GetValue("MESManufacture"));
            return Content("success");
            //var tenant = new MultiTenancy.Tenant()
            //{
            //    Name = "aa",
            //    TenancyName = "bb"
            //};
            //TenantRepository.Insert(tenant);
            //BaseTreeRepository.Insert(new BaseTree() { Discriminator = "abc",DisplayName="test",Sort=2 });
            //var objs = BaseTreeRepository.GetAll().IgnoreQueryFilters().ToList();

            //var objs = _userManager.GetAll().Where("(CreationTime-Convert.ToDateTime(LastModificationTime==null?\"2019-01-01\":\"2018-01-01\")).TotalDays<5").ToList();

            //return Content(objs.Count.ToString());
            return View();
        }
    }

}
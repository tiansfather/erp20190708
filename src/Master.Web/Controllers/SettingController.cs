using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Master.Configuration;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SettingController : MasterControllerBase
    {
        public IFeatureDependencyContext FeatureDependencyContext { get; set; }
        public ISettingDefinitionManager SettingDefinitionManager { get; set; }
        
        public async Task<IActionResult> Index()
        {


            var allSettingValues = await SettingManager.GetAllSettingValuesAsync();
            var settingValueDic=allSettingValues.ToDictionary(o => o.Name, o => (object)o.Value);
            var settings=SettingDefinitionManager.GetAllSettingDefinitions()
                .Where(o=> {
                    var result = true;
                    if(o.CustomData != null)
                    {
                        result = new SimpleFeatureDependency(false, (o.CustomData as SettingUIInfo).RequiredFeature?.Split(',')).IsSatisfied(FeatureDependencyContext);
                    }
                    return result;
                    })
                .ToList();
            ViewBag.SettingValues = settingValueDic;
            return View(settings);
        }
    }
}
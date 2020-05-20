using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Master.Configuration.Dictionaries;
using Master.Controllers;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BaseTreeController : MasterControllerBase
    {
        public BaseTreeManager BaseTreeManager { get; set; }
        public async Task<IActionResult> Add(string treeKey,int? parentId)
        {
            ViewBag.TreeKey = treeKey;
            ViewBag.ParentId = parentId;
            var dic = await Resolve<DictionaryManager>().GetUserDicByNameAsync("商品种类");
            ViewBag.Dictionary = dic == null ? new Dictionary<string, string>() : Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dic.DictionaryContent);
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var baseTree = await BaseTreeManager.GetByIdFromCacheAsync(id);
            var dic = await Resolve<DictionaryManager>().GetUserDicByNameAsync("商品种类");
            ViewBag.Dictionary = dic == null ? new Dictionary<string, string>() : Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dic.DictionaryContent);
            return View(baseTree);
        }
    }
}
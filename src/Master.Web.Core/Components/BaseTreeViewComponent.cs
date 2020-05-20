using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Configuration.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class BaseTreeViewComponent : MasterViewComponent
    {
        [UnitOfWork]
        public virtual async Task<IViewComponentResult> InvokeAsync(BaseTreeViewParam param)
        {
            using(var dictionaryManagerWrapper = IocManager.Instance.ResolveAsDisposable<DictionaryManager>())
            {
                var dic = await dictionaryManagerWrapper.Object.GetUserDicByNameAsync("商品种类");
                ViewBag.Dictionary = dic == null ? new Dictionary<string, string>() : Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dic.DictionaryContent);
            }
            
            return View(param);
        }
    }

    public class BaseTreeViewParam
    {
        /// <summary>
        /// 树标志，如UnitType
        /// </summary>
        public string TreeKey { get; set; }
        /// <summary>
        /// 树名称
        /// </summary>
        public string TreeName { get; set; }
        /// <summary>
        /// 是否显示对内
        /// </summary>
        public int ShowInner { get; set; } = 1;
        public bool EnableAdd { get; set; } = true;
        public bool EnableEdit { get; set; } = true;
        public bool EnableDelete { get; set; } = true;
        public bool EnableSearch { get; set; } = false;
    }
}

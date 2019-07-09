using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Web.Authorization;
using Abp.Web.Features;
using Abp.Web.Localization;
using Abp.Web.MultiTenancy;
using Abp.Web.Navigation;
using Abp.Web.Sessions;
using Abp.Web.Settings;
using Abp.Web.Timing;
using Castle.Core.Logging;
using Master.Authentication;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    /// <summary>
    /// This controller is used to create client side scripts
    /// to work with ABP.
    /// </summary>
    public class ScriptsController : MasterControllerBase
    {
        private readonly IMultiTenancyScriptManager _multiTenancyScriptManager;
        private readonly ISettingScriptManager _settingScriptManager;
        private readonly INavigationScriptManager _navigationScriptManager;
        private readonly ILocalizationScriptManager _localizationScriptManager;
        private readonly MyAuthorizationScriptManager _authorizationScriptManager;
        private readonly IFeaturesScriptManager _featuresScriptManager;
        private readonly ISessionScriptManager _sessionScriptManager;
        private readonly ITimingScriptManager _timingScriptManager;

        public ILogger Logger { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptsController(
            IMultiTenancyScriptManager multiTenancyScriptManager,
            ISettingScriptManager settingScriptManager,
            INavigationScriptManager navigationScriptManager,
            ILocalizationScriptManager localizationScriptManager,
            MyAuthorizationScriptManager authorizationScriptManager,
            IFeaturesScriptManager featuresScriptManager,
            ISessionScriptManager sessionScriptManager,
            ITimingScriptManager timingScriptManager)
        {
            _multiTenancyScriptManager = multiTenancyScriptManager;
            _settingScriptManager = settingScriptManager;
            _navigationScriptManager = navigationScriptManager;
            _localizationScriptManager = localizationScriptManager;
            _authorizationScriptManager = authorizationScriptManager;
            _featuresScriptManager = featuresScriptManager;
            _sessionScriptManager = sessionScriptManager;
            _timingScriptManager = timingScriptManager;
        }


        /// <summary>
        /// Gets all needed scripts.
        /// </summary>
        public async Task<ActionResult> GetScripts(string culture = "")
        {
            if (!culture.IsNullOrEmpty())
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            }

            Logger.Info("Start");

            var sb = new StringBuilder();

            sb.AppendLine(_multiTenancyScriptManager.GetScript());
            sb.AppendLine();
            Logger.Info("Start1");
            sb.AppendLine(_sessionScriptManager.GetScript());
            sb.AppendLine();
            Logger.Info("Start2");
            sb.AppendLine(_localizationScriptManager.GetScript());
            sb.AppendLine();
            Logger.Info("Start3");
            sb.AppendLine(await _featuresScriptManager.GetScriptAsync());
            sb.AppendLine();
            Logger.Info("Start4");
            sb.AppendLine(await _authorizationScriptManager.GetScriptAsync());
            sb.AppendLine();
            Logger.Info("Start5");
            sb.AppendLine(await _navigationScriptManager.GetScriptAsync());
            sb.AppendLine();
            Logger.Info("Start6");
            sb.AppendLine(await _settingScriptManager.GetScriptAsync());
            sb.AppendLine();
            Logger.Info("Start7");
            sb.AppendLine(await _timingScriptManager.GetScriptAsync());
            sb.AppendLine();
            Logger.Info("Start8");
            sb.AppendLine(GetTriggerScript());
            Logger.Info("Start9");
            return Content(sb.ToString(), "application/x-javascript", Encoding.UTF8);
        }

        private static string GetTriggerScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.event.trigger('abp.dynamicScriptsInitialized');");
            script.Append("})();");

            return script.ToString();
        }
    }

    public class MyAuthorizationScriptManager : IAuthorizationScriptManager, ITransientDependency
    {
        /// <inheritdoc/>
        public IAbpSession AbpSession { get; set; }
        public ILogger Logger { get; set; }

        private readonly IPermissionManager _permissionManager;

        public IPermissionChecker PermissionChecker { get; set; }

        /// <inheritdoc/>
        public MyAuthorizationScriptManager(IPermissionManager permissionManager)
        {
            AbpSession = NullAbpSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;

            _permissionManager = permissionManager;
        }

        /// <inheritdoc/>
        public async Task<string> GetScriptAsync()
        {
            Logger.Info("StartGetScript1");
            var allPermissionNames = _permissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            Logger.Info("StartGetScript2");
            var grantedPermissionNames = new List<string>();

            if (AbpSession.UserId.HasValue)
            {
                using (var userManagerWrapper = IocManager.Instance.ResolveAsDisposable<UserManager>())
                {
                    var grantedPermissions = await userManagerWrapper.Object.GetGrantedPermissionsAsync(AbpSession.UserId.Value);
                    grantedPermissionNames = grantedPermissions.Select(o => o.Name).ToList();
                }
                    //foreach (var permissionName in allPermissionNames)
                    //{
                    //    if (await PermissionChecker.IsGrantedAsync(permissionName))
                    //    {
                    //        grantedPermissionNames.Add(permissionName);
                    //    }
                    //}
            }
            Logger.Info("StartGetScript3");
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine();

            script.AppendLine("    abp.auth = abp.auth || {};");

            script.AppendLine();

            AppendPermissionList(script, "allPermissions", allPermissionNames);

            script.AppendLine();

            AppendPermissionList(script, "grantedPermissions", grantedPermissionNames);

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }

        private static void AppendPermissionList(StringBuilder script, string name, IReadOnlyList<string> permissions)
        {
            script.AppendLine("    abp.auth." + name + " = {");

            for (var i = 0; i < permissions.Count; i++)
            {
                var permission = permissions[i];
                if (i < permissions.Count - 1)
                {
                    script.AppendLine("        '" + permission + "': true,");
                }
                else
                {
                    script.AppendLine("        '" + permission + "': true");
                }
            }

            script.AppendLine("    };");
        }
    }
}
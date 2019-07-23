using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public static class SettingNames
    {
        public const string MenuSetting = "Menu";
        public const string SoftTitle = "App.SoftTitle";
    }
    public class MasterSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //var interGroup = new SettingDefinitionGroup("InterSetting", L("内部设置"));
            //group设为null则不在设置页面中出现
            var menuSettingDefinition = new SettingDefinition(SettingNames.MenuSetting, "", L("菜单"), group: null, scopes: SettingScopes.Tenant | SettingScopes.User);


            var group = new SettingDefinitionGroup("Core", L("基本设置"));
            var group2 = new SettingDefinitionGroup("Home", L("首页轮播"));
            return new SettingDefinition[]
            {
                menuSettingDefinition,
                new SettingDefinition(SettingNames.SoftTitle, "管理系统",L("系统标题"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true),
                new SettingDefinition("HomPics", "",L("首页轮播"),group, scopes: SettingScopes.Tenant , isVisibleToClients: true
               ,customData:new SettingUIInfo(){ ColumnType=Module.ColumnTypes.Text,Renderer="lay-homepics"}),
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}

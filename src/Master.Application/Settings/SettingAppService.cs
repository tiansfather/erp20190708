using Abp.Authorization;
using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Settings
{
    [AbpAuthorize]
    public class SettingAppService:MasterAppServiceBase
    {
        /// <summary>
        /// 更新账套的设置
        /// </summary>
        /// <param name="settingDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateSettingsForTenant(List<SettingDto> settingDtos)
        {
            foreach(var settingDto in settingDtos)
            {
                await SettingManager.ChangeSettingForTenantAsync(AbpSession.TenantId.Value, settingDto.Name, settingDto.Value);
            }
            
        }
    }
}

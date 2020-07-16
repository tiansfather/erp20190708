using Abp.Authorization;
using Abp.Configuration;
using Master.Authentication;
using Master.Configuration;
using Master.Settings;
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
                //await SettingManager.ChangeSettingForTenantAsync(AbpSession.TenantId.Value, settingDto.Name, settingDto.Value);
                await SettingManager.ChangeSettingForApplicationAsync(settingDto.Name, settingDto.Value);
            }
            
        }

        public virtual async Task<object> GetFeePointSetting()
        {
            var setting = await SettingManager.GetSettingValueForApplicationAsync(SettingNames.FeePointSetting);
            if (string.IsNullOrEmpty(setting))
            {
                return new List<string>();
            }
            else
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FeePointSetting>>(setting);
                result.Reverse();
                return result;
            }
        }
        public virtual async Task AddFeePoint(DateTime dateTime)
        {
            List<FeePointSetting> feePointSettings = new List<FeePointSetting>();
            var setting = await SettingManager.GetSettingValueForApplicationAsync(SettingNames.FeePointSetting);
            if (!string.IsNullOrEmpty(setting))
            {
                feePointSettings= Newtonsoft.Json.JsonConvert.DeserializeObject<List<FeePointSetting>>(setting);
            }
            feePointSettings.Add(new FeePointSetting()
            {
                FeeDate = dateTime,
                CreationTime = DateTime.Now,
                Creator = AbpSession.Name()
            });
            await SettingManager.ChangeSettingForApplicationAsync(SettingNames.FeePointSetting, Newtonsoft.Json.JsonConvert.SerializeObject(feePointSettings));
        }
        public virtual async Task RemoveFeePoint(int index)
        {
            List<FeePointSetting> feePointSettings = new List<FeePointSetting>();
            var setting = await SettingManager.GetSettingValueForApplicationAsync(SettingNames.FeePointSetting);
            feePointSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FeePointSetting>>(setting);
            feePointSettings.RemoveAt(index);
            await SettingManager.ChangeSettingForApplicationAsync(SettingNames.FeePointSetting, Newtonsoft.Json.JsonConvert.SerializeObject(feePointSettings));
        }
    }
}

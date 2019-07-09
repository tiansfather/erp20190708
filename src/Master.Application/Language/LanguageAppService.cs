using Abp.Localization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Language
{
    public class LanguageAppService:MasterAppServiceBase<ApplicationLanguageText,long>
    {
        public ILanguageManager LanguageManager { get; set; }


        /// <summary>
        /// 增加翻译
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual async Task AddText(string text)
        {
            var manager = Manager as ApplicationLanguageTextManager;
            var textLines = text.Split('\n');
            foreach(var lineText in textLines)
            {
                if (lineText.IndexOf(":") <= 0)
                {
                    throw new UserFriendlyException("每行元素必须用:分隔");
                }
                var key = lineText.Split(':')[0];
                var value = lineText.Split(':')[1];

                var existLanguageText = await manager.GetAll().Where(o => o.Key == key).SingleOrDefaultAsync();
                if (existLanguageText == null)
                {
                    await manager.UpdateStringAsync(null, MasterConsts.LocalizationSourceName, new CultureInfo("en"), key, value);
                }
                else
                {
                    existLanguageText.Value = value;
                }
            }
        }

        public virtual async Task UpdateField(int id,string field,string value)
        {
            var text = await Manager.GetByIdAsync(id);
            if (field == "key")
            {
                text.Key = value;
            }
            else
            {
                text.Value = value;
            }

        }

        /// <summary>
        /// 获取所有可用语种
        /// </summary>
        /// <returns></returns>
        public virtual LanguageDto Get()
        {
            var dto = new LanguageDto()
            {
                LanguageInfos = LanguageManager.GetLanguages().Where(o => !o.IsDisabled),
                CurrentLanguage = LanguageManager.CurrentLanguage.Name
            };
            return dto;
        }
    }
}

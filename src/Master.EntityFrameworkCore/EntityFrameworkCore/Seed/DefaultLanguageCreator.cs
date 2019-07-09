using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.EntityFrameworkCore.Seed
{
    public class DefaultLanguagesCreator
    {
        public static List<ApplicationLanguage> InitialLanguages { get; private set; }

        private readonly MasterDbContext _context;

        static DefaultLanguagesCreator()
        {
            InitialLanguages = new List<ApplicationLanguage>
            {
                new ApplicationLanguage(null, "zh-cn", "中文", ""),
                new ApplicationLanguage(null, "en", "English", ""),
                
            };
        }

        public DefaultLanguagesCreator(MasterDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();
        }

        private void CreateLanguages()
        {
            foreach (var language in InitialLanguages)
            {
                AddLanguageIfNotExists(language);
            }
        }

        private void AddLanguageIfNotExists(ApplicationLanguage language)
        {
            if (_context.ApplicationLanguage.Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
            {
                return;
            }

            _context.ApplicationLanguage.Add(language);

            _context.SaveChanges();
        }
    }
}

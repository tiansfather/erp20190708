using Abp.Localization.Dictionaries;

namespace Master.Configuration
{
    /// <summary>
    /// Used to configure language management.
    /// </summary>
    public interface ILanguageManagementConfig
    {
        /// <summary>
        /// Enables the database localization.
        /// Replaces all <see cref="IDictionaryBasedLocalizationSource"/> localization sources
        /// with database based localization source.
        /// </summary>
        void EnableDbLocalization();
    }
}
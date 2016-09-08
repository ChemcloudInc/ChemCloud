using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
    public interface ISiteSettingService : IService, IDisposable
    {
        SiteSettingsInfo GetSiteSettings();

        void SaveSetting(string key, object value);

        void SetSiteSettings(SiteSettingsInfo siteSettingsInfo);

        string GetSiteValue(string key);

        SiteSettingsInfo GetSiteSettings(string key);

        void UpdateSiteSettings(SiteSettingsInfo siteSettingsInfo);
    }
}
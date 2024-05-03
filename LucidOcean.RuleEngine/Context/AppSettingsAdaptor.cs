/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Configuration;

namespace LucidOcean.RuleEngine.Context
{
    /// <summary>
    /// Wraps App Settings to provide a Key based generic Get/Set
    /// </summary>
    public class AppSettingsAdaptor : IConfigurationAccessor
    {
		

        private const string AppSettingsConfigName = "appSettings";


        public string GetValue(string settingName)
        {
            if (ConfigurationManager.AppSettings[settingName] != null)
                return ConfigurationManager.AppSettings[settingName].ToString();
            else
                return "";

        }

        public bool SetValue(string settingName, string settingValue)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config != null)
            {
                AppSettingsSection appSettings = config.AppSettings;

                KeyValueConfigurationElement setting = appSettings.Settings[settingName];

                if (setting != null)
                {
                    setting.Value = settingValue;

                    config.Save(ConfigurationSaveMode.Modified);

                    ConfigurationManager.RefreshSection(AppSettingsConfigName);

                    return true;
                }
            }

            return false;
        }

        
    }
}

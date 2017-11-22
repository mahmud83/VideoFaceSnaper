using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace VideoFace.Common.Util
{
    public class ConfigurationHelper
    {
        public static T GetValue<T>(string configurationAppSettingName, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[configurationAppSettingName] != null)
            {
                return (T)Convert.ChangeType(ConfigurationManager.AppSettings[configurationAppSettingName], typeof(T));
            }
            return defaultValue;
        }
    }
}

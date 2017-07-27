

namespace Common.Util
{
    using System;
    using System.Configuration;

    using Extensions;

    public class Key
    {
        public class AppSettings
        {
            public string ScheduleIdPrefix = "schedule_id_prefix";
        }

        public class ConnectionStrs
        {
            public const string HangfireConnStr = "hangfireConnStr";
        }
        
    }
    
    public class ConfigHelper
    {
        private static Key.AppSettings keys = new Key.AppSettings();


        public static string GetSetting(Func<Key.AppSettings, string> keyFn, string defaultVal="")
        {
            var key = keyFn(keys);

            return GetSetting(key, defaultVal);
        }

        public static int GetIntSetting(Func<Key.AppSettings, string> keyFn, int defaultVal = 0)
        {
            var key = keyFn(keys);

            return GetIntSetting(key, defaultVal);
        }


        public static string GetSetting(string key, string defaultVal = "")
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                var result = ConfigurationManager.AppSettings.Get(key);
                return string.IsNullOrWhiteSpace(result) ? defaultVal : result;
            }

            return string.Empty;
        }

        public static int GetIntSetting(string key, int defaultVal = 0)
        {
            var s = GetSetting(key);
            return s.ToInt(defaultVal);
        }

    }
}

using System;

namespace PoohAPI.Common
{
    public static class ConfigurationReader
    {
        private static string GetConfigurationSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public static string TestValue => GetConfigurationSetting(nameof(TestValue));

    }
}

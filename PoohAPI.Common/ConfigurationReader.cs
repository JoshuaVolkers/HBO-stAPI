using System;

namespace PoohAPI.Common
{
    public sealed class ConfigurationReader
    {

        private static string GetConfigurationSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public static string TestValue => GetConfigurationSetting(nameof(TestValue));

    }
}

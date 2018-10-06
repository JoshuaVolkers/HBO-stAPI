using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace PoohAPI.Common
{
    public static class ConfigurationReader
    {
        private static string GetConfigurationSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string TestValue => GetConfigurationSetting(nameof(TestValue));

    }
}

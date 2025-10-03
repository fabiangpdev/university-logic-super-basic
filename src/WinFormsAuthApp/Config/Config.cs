using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WinFormsAuthApp.Config
{
    public static class Config
    {
        private static IConfigurationRoot? _configuration;

        public static IConfigurationRoot GetConfiguration()
        {
            if (_configuration != null) return _configuration;

            var basePath = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
            return _configuration;
        }

        public static string GetConnectionString()
        {
            return GetConfiguration().GetConnectionString("Default")!;
        }
    }
}



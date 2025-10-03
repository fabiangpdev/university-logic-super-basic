using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WinFormsAuthApp.Config
{
    public static class Config
    {
        private static IConfigurationRoot? _configuration;
        public static class Session
        {
            public static int UserId { get; set; }
            public static string UserName { get; set; } = string.Empty;
            public static string Role { get; set; } = "user";
            public static bool IsAdmin => string.Equals(Role, "admin", StringComparison.OrdinalIgnoreCase);
            public static void Clear()
            {
                UserId = 0;
                UserName = string.Empty;
                Role = "user";
            }
        }

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



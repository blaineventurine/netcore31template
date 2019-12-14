using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Common.Helpers
{
    public static class ConfigFileLoader
    {
        public static IConfigurationRoot GetAppSettings(IHostEnvironment env = null)
        {
            var environmentName = "Localhost";
            if (env != null)
                environmentName = env.EnvironmentName;
            var jsonFileName = $"appsettings.{environmentName}.json";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFileName, false, true)
                .Build();

            return config;
        }
    }
}

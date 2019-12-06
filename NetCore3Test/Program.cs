using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NetCore3Test
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        var env = builderContext.HostingEnvironment;
                        Console.WriteLine(env.EnvironmentName);
                        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                        config.AddEnvironmentVariables();
                    });
                    webBuilder.UseKestrel(options =>
                    {
                        //options.Limits.MaxRequestBodySize = 524_288_000; //500MB
                        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                        options.AddServerHeader = false;
                    });
                });
    }
}

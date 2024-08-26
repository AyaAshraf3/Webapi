using Exchange;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Exchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

      
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Load the configuration from appsettings.json
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    string? baseUrl, iisUrl, httpUrl, Hostname;
                    int port;
                    setConfiguration(hostContext, out baseUrl, out port, out iisUrl, out httpUrl, out Hostname);

                    // Log the configuration settings (for debugging)
                    var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
                    logger.LogInformation($"Base URL: {baseUrl}");
                    logger.LogInformation($"Port: {port}");
                    logger.LogInformation($"IIS URL: {iisUrl}");
                    logger.LogInformation($"HTTP URL: {httpUrl}");

                    // Add the worker service
                    // Add the worker service and pass the hostname
                    services.AddHostedService<Worker>(provider =>
                        new Worker(provider.GetRequiredService<ILogger<Worker>>(), Hostname));
                });

        internal static void setConfiguration(HostBuilderContext hostContext, out string? baseUrl, out int port, out string? iisUrl, out string? httpUrl, out string? hostName)
        {
            // Retrieve the configuration settings
            var configuration = hostContext.Configuration;
            baseUrl = configuration.GetValue<string>("AppSettings:BaseUrl");
            port = configuration.GetValue<int>("AppSettings:Port");
            var backgroundUrl = configuration.GetValue<string>("AppSettings:backgroundUrl");
            iisUrl = configuration.GetValue<string>("AppSettings:IISUrl");
            httpUrl = configuration.GetValue<string>("AppSettings:HttpUrl");
            hostName = configuration.GetValue<string>("AppSettings:Hostname");
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GenericConsoleTemplate;
class Program
{
    static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services
                    .AddHostedService<LifetimeEventsHostedService>()
                    //.AddHostedService<TimerHostedService>()
                    //.AddHostedService<CmdBackgroundService>()
                    .AddHostedService<ConsoleHostedService>();
                
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                logging
                    .AddDebug()
                    .AddConsole();
            })
            .ConfigureAppConfiguration((hostContext, configuration) =>
            {
                configuration
                    // Site list in ini file
                    //.AddIniFile("websites.ini")

                    // Config JsonFile
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)

                    // Override config for dev environment for example
                    .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", false, true)

                    // Override config with environment variables
                    .AddEnvironmentVariables()

                    .AddCommandLine(args);
            })
            .RunConsoleAsync();
    }
}
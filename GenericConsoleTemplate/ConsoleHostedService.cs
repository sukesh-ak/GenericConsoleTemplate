using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericConsoleTemplate;
public class ConsoleHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private int? _exitCode=0;

    public ConsoleHostedService(ILogger<ConsoleHostedService> logger,
                            IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        #region App Lifetime Events
        // Started
        _appLifetime.ApplicationStarted.Register(async () =>
            {
                _logger.LogInformation("Started has been called.");

                await DoSomething();

            });

        // Stopping
        _appLifetime.ApplicationStopping.Register(() =>
        {
            _logger.LogInformation("Stopping has been called.");
        });

        // Stopped
        _appLifetime.ApplicationStopped.Register(() =>
        {
            _logger.LogInformation("Stopped has been called.");
        });
        #endregion
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Exiting in StopAsync");
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }

    private async Task DoSomething()
    {
        try
        {
            _logger.LogInformation("Starting DoSomething");
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Wow we have exception {ex.Message}");
            _exitCode = -1;
        }
        finally
        {
            // Gracefully exiting
            _logger.LogInformation("Time to stop DoingThings");
            _appLifetime.StopApplication(); 
        }


        _logger.LogInformation("Ending DoSomething");


    }
}

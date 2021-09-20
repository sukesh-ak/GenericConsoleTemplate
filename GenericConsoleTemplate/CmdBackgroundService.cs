using Microsoft.Extensions.Logging;

namespace GenericConsoleTemplate;

public class CmdBackgroundService : BackgroundService
{
    private readonly ILogger<CmdBackgroundService> _logger;

    public CmdBackgroundService(ILogger<CmdBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CmdBackgroundService is starting");
        stoppingToken.Register(() =>
        {
            _logger.LogInformation("CmdBackgroundService task is stopping");
        });

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug($"CmdBackgroundService task doing background work.");

            await Task.Delay(1000,stoppingToken);
        }
        _logger.LogInformation("CmdBackgroundService is stopping");
    }
}

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

        _logger.LogInformation("Started has been called.");
        Console.WriteLine("ENTER Command: cool for work and quit for exit");
        string? cmd;
        
        while(!cancellationToken.IsCancellationRequested)
        {
            Console.Write(">");
            cmd = Console.ReadLine();
            if (cmd=="quit") _appLifetime.StopApplication();
            else if (cmd == "cool")
            {
                DoWork(cancellationToken).Wait();
            }
        }
            //_ = DoWork(cancellationToken);


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Exiting in StopAsync");
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting DoWork");
            await Task.Delay(100, cancellationToken);
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
            //_appLifetime.StopApplication(); 
        }


        _logger.LogInformation("Ending DoWork");
    }
}

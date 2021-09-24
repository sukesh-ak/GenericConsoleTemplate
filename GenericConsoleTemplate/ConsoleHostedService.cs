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
        Console.WriteLine("Welcome to Interactive CLI");
        string navScope = "ROOT";
        string breadcrumps = ">";
        
        while(!cancellationToken.IsCancellationRequested)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{breadcrumps} ");
            Console.ResetColor();
            string? cmd = Console.ReadLine();
            ArgumentNullException.ThrowIfNull(cmd); // Short of if null check

            switch (cmd.ToUpperInvariant())
            {
                case "QUIT":
                    _appLifetime.StopApplication();
                    break;
                case "NAV1":
                case "NAV2":
                    navScope = cmd.ToUpperInvariant();
                    string camelcase = cmd.ToUpperInvariant().Substring(0, 1) + cmd.ToLowerInvariant().Substring(1);
                    breadcrumps = $"{camelcase}>";
                    break;
                case "EXIT":
                    navScope = "ROOT";
                    breadcrumps = ">";
                    break;
                case "HELP":
                    Console.WriteLine("Show help");
                    break;
                case "JUNK":
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine($"Number {i}");
                    }
                    break;
                case "RUN":
                    Console.Write("Analyzing...");
                    Console.CursorVisible = false;
                    for (int i = 0; i < 10; i++)
                    {
                        if (i % 2 == 0)
                            Console.Write("\\");
                        else
                            Console.Write("/");
                        Console.CursorLeft = Console.CursorLeft - 1;
                        //Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    Console.Write("Done!");
                    Console.CursorVisible = true;
                    Console.WriteLine();
                    break;
                case "PLUGINS":
                    /*
                     * If navScope is ROOT then show all
                     * If navScope is <scope> then show all scoped plugins+commands (filter)
                     */
                    //Console.WriteLine($"Command: {cmd}");
                    Console.WriteLine("Plugins available:");
                    Console.WriteLine(" Ping");
                    Console.WriteLine(" Tracert");
                    break;
                case "DOWORK":
                    DoWork(cancellationToken).Wait();
                    break;
                default:
                    if (!String.IsNullOrEmpty(cmd))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Invalid command: {cmd}");
                        Console.ResetColor();
                    }
                    break;
            }
        }

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

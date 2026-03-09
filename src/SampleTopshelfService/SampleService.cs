namespace SampleTopshelfService;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

class SampleWorker : BackgroundService
{
    readonly ILogger<SampleWorker> _logger;
    readonly IHostApplicationLifetime _lifetime;
    readonly SampleServiceOptions _options;

    public SampleWorker(
        ILogger<SampleWorker> logger,
        IHostApplicationLifetime lifetime,
        IOptions<SampleServiceOptions> options)
    {
        _logger = logger;
        _lifetime = lifetime;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SampleWorker Starting...");

        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

        if (_options.ThrowOnStart)
        {
            _logger.LogInformation("Throwing as requested on start");
            throw new InvalidOperationException("Throw on Start Requested");
        }

        _logger.LogInformation("SampleWorker Started — will request stop in 3 seconds");

        await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);

        if (_options.ThrowUnhandled)
            throw new InvalidOperationException("Throw Unhandled In Background Thread");

        _logger.LogInformation("Requesting stop");
        _lifetime.StopApplication();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SampleWorker Stopping");

        if (_options.ThrowOnStop)
            throw new InvalidOperationException("Throw on Stop Requested!");

        await base.StopAsync(cancellationToken);

        _logger.LogInformation("SampleWorker Stopped");
    }
}
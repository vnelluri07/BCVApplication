using BeersCheersVasis.Repository;

namespace BeersCheersVasis.API.Internal;

public sealed class ScriptSchedulerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScriptSchedulerService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(15);

    public ScriptSchedulerService(IServiceScopeFactory scopeFactory, ILogger<ScriptSchedulerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IScriptRepository>();
                var count = await repo.PublishScheduledScriptsAsync(stoppingToken);
                if (count > 0)
                    _logger.LogInformation("Published {Count} scheduled script(s)", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Script scheduler error");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}

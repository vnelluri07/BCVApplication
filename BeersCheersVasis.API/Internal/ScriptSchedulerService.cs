using BeersCheersVasis.Repository;
using BeersCheersVasis.Services;

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
                var publishedIds = await repo.PublishScheduledScriptsAsync(stoppingToken);
                if (publishedIds.Count > 0)
                {
                    _logger.LogInformation("Published {Count} scheduled script(s)", publishedIds.Count);
                    var scriptService = scope.ServiceProvider.GetRequiredService<IScriptService>();
                    var backupService = scope.ServiceProvider.GetRequiredService<IScriptBackupService>();
                    foreach (var id in publishedIds)
                    {
                        try
                        {
                            var script = await scriptService.GetScriptAsync(id, stoppingToken);
                            if (script is null) continue;
                            await backupService.BackupScriptAsync(
                                new BackupPayload(script.Id, script.Title, script.Content, script.CategoryName, script.PublishedDate ?? DateTime.UtcNow),
                                stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Backup failed for scheduled script {ScriptId}", id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Script scheduler error");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}

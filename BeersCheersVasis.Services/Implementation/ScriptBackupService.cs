using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repository;
using Microsoft.Extensions.Logging;

namespace BeersCheersVasis.Services.Implementation;

public sealed class ScriptBackupService(
    IEnumerable<IBackupProvider> providers,
    IScriptBackupRepository backupRepository,
    ILogger<ScriptBackupService> logger) : IScriptBackupService
{
    public async Task BackupScriptAsync(BackupPayload payload, CancellationToken cancellationToken)
    {
        foreach (var provider in providers)
        {
            var backup = new ScriptBackup
            {
                ScriptId = payload.ScriptId,
                Provider = provider.ProviderName,
                BackedUpAt = DateTime.UtcNow,
                Status = "Pending"
            };

            try
            {
                var result = await provider.BackupAsync(payload, cancellationToken);
                backup.Status = result.Success ? "Success" : "Failed";
                backup.ExternalId = result.ExternalId;
                backup.ExternalUrl = result.ExternalUrl;
                backup.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Backup failed for script {ScriptId} via {Provider}", payload.ScriptId, provider.ProviderName);
                backup.Status = "Failed";
                backup.ErrorMessage = ex.Message;
            }

            await backupRepository.AddAsync(backup, cancellationToken);
        }
    }
}

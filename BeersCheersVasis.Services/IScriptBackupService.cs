namespace BeersCheersVasis.Services;

public interface IScriptBackupService
{
    Task BackupScriptAsync(BackupPayload payload, CancellationToken cancellationToken);
}

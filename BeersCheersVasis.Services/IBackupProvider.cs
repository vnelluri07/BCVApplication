namespace BeersCheersVasis.Services;

public interface IBackupProvider
{
    string ProviderName { get; }
    Task<BackupResult> BackupAsync(BackupPayload payload, CancellationToken cancellationToken);
}

public sealed record BackupPayload(int ScriptId, string Title, string HtmlContent, string? CategoryName, DateTime PublishedDate, string? PreviousExternalId = null);

public sealed record BackupResult(bool Success, string? ExternalId = null, string? ExternalUrl = null, string? ErrorMessage = null);

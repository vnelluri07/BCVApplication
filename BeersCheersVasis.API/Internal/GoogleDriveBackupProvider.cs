using BeersCheersVasis.API.Configuration;
using BeersCheersVasis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace BeersCheersVasis.API.Internal;

public sealed class GoogleDriveBackupProvider(BackupSettings settings) : IBackupProvider
{
    private readonly GoogleDriveBackupSettings _cfg = settings.GoogleDrive;

    public string ProviderName => "GoogleDrive";

    public async Task<BackupResult> BackupAsync(BackupPayload payload, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_cfg.ServiceAccountJson))
            return new BackupResult(false, ErrorMessage: "Google Drive backup not configured");

        var credential = GoogleCredential
            .FromJson(_cfg.ServiceAccountJson)
            .CreateScoped(DriveService.Scope.DriveFile);

        using var driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "BCVApp"
        });

        var html = BuildHtml(payload);
        var fileName = $"{payload.ScriptId:D4} - {payload.Title}";

        // Check if file already exists (by name in folder)
        var existingId = await FindExistingFileAsync(driveService, fileName, cancellationToken);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));

        if (existingId is not null)
        {
            var updateRequest = driveService.Files.Update(new(), existingId, stream, "text/html");
            var updateResult = await updateRequest.UploadAsync(cancellationToken);
            if (updateResult.Status != Google.Apis.Upload.UploadStatus.Completed)
                return new BackupResult(false, ErrorMessage: updateResult.Exception?.Message ?? "Upload failed");

            return new BackupResult(true, ExternalId: existingId, ExternalUrl: $"https://drive.google.com/file/d/{existingId}/view");
        }

        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName,
            MimeType = "application/vnd.google-apps.document",
            Parents = string.IsNullOrEmpty(_cfg.FolderId) ? null : [_cfg.FolderId]
        };

        var createRequest = driveService.Files.Create(fileMetadata, stream, "text/html");
        createRequest.Fields = "id, webViewLink";
        var result = await createRequest.UploadAsync(cancellationToken);

        if (result.Status != Google.Apis.Upload.UploadStatus.Completed)
            return new BackupResult(false, ErrorMessage: result.Exception?.Message ?? "Upload failed");

        var file = createRequest.ResponseBody;
        return new BackupResult(true, ExternalId: file.Id, ExternalUrl: file.WebViewLink);
    }

    private async Task<string?> FindExistingFileAsync(DriveService service, string name, CancellationToken cancellationToken)
    {
        var query = $"name = '{name.Replace("'", "\\'")}' and trashed = false";
        if (!string.IsNullOrEmpty(_cfg.FolderId))
            query += $" and '{_cfg.FolderId}' in parents";

        var listRequest = service.Files.List();
        listRequest.Q = query;
        listRequest.Fields = "files(id)";
        listRequest.PageSize = 1;

        var files = await listRequest.ExecuteAsync(cancellationToken);
        return files.Files?.FirstOrDefault()?.Id;
    }

    private static string BuildHtml(BackupPayload p) => $"""
        <!DOCTYPE html>
        <html>
        <head><title>{System.Net.WebUtility.HtmlEncode(p.Title)}</title></head>
        <body>
        <h1>{System.Net.WebUtility.HtmlEncode(p.Title)}</h1>
        <p><em>Category: {System.Net.WebUtility.HtmlEncode(p.CategoryName ?? "Uncategorized")} | Published: {p.PublishedDate:yyyy-MM-dd} | Backed up: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC</em></p>
        <hr/>
        {p.HtmlContent}
        </body>
        </html>
        """;
}

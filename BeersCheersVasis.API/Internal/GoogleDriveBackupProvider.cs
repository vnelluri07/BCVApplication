using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.API.Configuration;
using BeersCheersVasis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.API.Internal;

public sealed class GoogleDriveBackupProvider(IServiceScopeFactory scopeFactory, GoogleAuthSettings googleAuth) : IBackupProvider
{
    private const string RefreshTokenKey = "google_drive_refresh_token";
    private const string FolderIdKey = "google_drive_folder_id";
    private const string FolderName = "BCV Backups";

    public string ProviderName => "GoogleDrive";

    public async Task<BackupResult> BackupAsync(BackupPayload payload, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IdbContext>();

        var refreshToken = (await db.SiteSettings.FirstOrDefaultAsync(s => s.Key == RefreshTokenKey, cancellationToken))?.Value;
        if (string.IsNullOrEmpty(refreshToken))
            return new BackupResult(false, ErrorMessage: "Google Drive not configured — admin must log in first");

        using var driveService = BuildDriveService(refreshToken);
        var folderId = await GetOrCreateFolderAsync(driveService, db, cancellationToken);
        var fileName = $"{payload.ScriptId:D4} - {payload.Title}";
        var html = BuildHtml(payload);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));

        // Update existing file by stored ID, or search by name as fallback
        var existingId = payload.PreviousExternalId
            ?? await FindFileAsync(driveService, fileName, folderId, cancellationToken);
        if (existingId is not null)
        {
            var req = driveService.Files.Update(new(), existingId, stream, "text/html");
            var res = await req.UploadAsync(cancellationToken);
            return res.Status == Google.Apis.Upload.UploadStatus.Completed
                ? new BackupResult(true, ExternalId: existingId, ExternalUrl: $"https://docs.google.com/document/d/{existingId}/edit")
                : new BackupResult(false, ErrorMessage: res.Exception?.Message ?? "Upload failed");
        }

        var meta = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName,
            MimeType = "application/vnd.google-apps.document",
            Parents = [folderId]
        };
        var create = driveService.Files.Create(meta, stream, "text/html");
        create.Fields = "id";
        var result = await create.UploadAsync(cancellationToken);

        if (result.Status != Google.Apis.Upload.UploadStatus.Completed)
            return new BackupResult(false, ErrorMessage: result.Exception?.Message ?? "Upload failed");

        var id = create.ResponseBody.Id;
        return new BackupResult(true, ExternalId: id, ExternalUrl: $"https://docs.google.com/document/d/{id}/edit");
    }

    private DriveService BuildDriveService(string refreshToken) => new(new BaseClientService.Initializer
    {
        HttpClientInitializer = new UserCredential(
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets { ClientId = googleAuth.ClientId, ClientSecret = googleAuth.ClientSecret },
                Scopes = [DriveService.Scope.DriveFile]
            }),
            "admin",
            new TokenResponse { RefreshToken = refreshToken }),
        ApplicationName = "BCVApp"
    });

    private static async Task<string> GetOrCreateFolderAsync(DriveService service, IdbContext db, CancellationToken ct)
    {
        var setting = await db.SiteSettings.FirstOrDefaultAsync(s => s.Key == FolderIdKey, ct);
        if (!string.IsNullOrEmpty(setting?.Value))
        {
            // Verify folder still exists
            try { await service.Files.Get(setting.Value).ExecuteAsync(ct); return setting.Value; }
            catch { /* folder deleted, recreate */ }
        }

        var folder = new Google.Apis.Drive.v3.Data.File { Name = FolderName, MimeType = "application/vnd.google-apps.folder" };
        var created = await service.Files.Create(folder).ExecuteAsync(ct);

        if (setting is null)
            db.SiteSettings.Add(new BeersCheersVasis.Data.Entities.SiteSetting { Key = FolderIdKey, Value = created.Id });
        else
            setting.Value = created.Id;
        await db.SaveChangesAsync(ct);

        return created.Id;
    }

    private static async Task<string?> FindFileAsync(DriveService service, string name, string folderId, CancellationToken ct)
    {
        var list = service.Files.List();
        list.Q = $"name = '{name.Replace("'", "\\'")}' and '{folderId}' in parents and trashed = false";
        list.Fields = "files(id)";
        list.PageSize = 1;
        var files = await list.ExecuteAsync(ct);
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

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using BeersCheersVasis.API.Configuration;
using BeersCheersVasis.Services;

namespace BeersCheersVasis.API.Internal;

public sealed partial class GitHubBackupProvider(IHttpClientFactory httpClientFactory, BackupSettings settings) : IBackupProvider
{
    private readonly GitHubBackupSettings _cfg = settings.GitHub;

    public string ProviderName => "GitHub";

    public async Task<BackupResult> BackupAsync(BackupPayload payload, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_cfg.PersonalAccessToken) || string.IsNullOrEmpty(_cfg.Owner))
            return new BackupResult(false, ErrorMessage: "GitHub backup not configured");

        var slug = SlugRegex().Replace(payload.Title.ToLowerInvariant(), "-").Trim('-');
        var path = $"scripts/{payload.ScriptId:D4}-{slug}.md";

        var markdown = BuildMarkdown(payload);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(markdown));

        using var httpClient = httpClientFactory.CreateClient("GitHubBackup");

        // Check if file exists (to get SHA for update)
        var existingSha = await GetFileShaAsync(httpClient, path, cancellationToken);

        var body = new Dictionary<string, string>
        {
            ["message"] = $"Backup: {payload.Title} ({DateTime.UtcNow:yyyy-MM-dd HH:mm})",
            ["content"] = base64
        };
        if (existingSha is not null)
            body["sha"] = existingSha;

        var url = $"https://api.github.com/repos/{_cfg.Owner}/{_cfg.Repo}/contents/{path}";
        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cfg.PersonalAccessToken);
        request.Headers.UserAgent.ParseAdd("BCVApp/1.0");
        request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        using var response = await httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            return new BackupResult(false, ErrorMessage: $"GitHub API {(int)response.StatusCode}: {responseBody[..Math.Min(200, responseBody.Length)]}");

        var doc = JsonDocument.Parse(responseBody);
        var htmlUrl = doc.RootElement.GetProperty("content").GetProperty("html_url").GetString();
        var sha = doc.RootElement.GetProperty("content").GetProperty("sha").GetString();

        return new BackupResult(true, ExternalId: sha, ExternalUrl: htmlUrl);
    }

    private async Task<string?> GetFileShaAsync(HttpClient httpClient, string path, CancellationToken cancellationToken)
    {
        var url = $"https://api.github.com/repos/{_cfg.Owner}/{_cfg.Repo}/contents/{path}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cfg.PersonalAccessToken);
        request.Headers.UserAgent.ParseAdd("BCVApp/1.0");

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("sha").GetString();
    }

    private static string BuildMarkdown(BackupPayload p) => $"""
        ---
        id: {p.ScriptId}
        title: "{p.Title.Replace("\"", "\\\"")}"
        category: "{p.CategoryName ?? "Uncategorized"}"
        published: {p.PublishedDate:yyyy-MM-ddTHH:mm:ssZ}
        backed_up: {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}
        ---

        # {p.Title}

        {p.HtmlContent}
        """;

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex SlugRegex();
}

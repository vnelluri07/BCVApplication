using System.Net.Http.Json;
using System.Text.RegularExpressions;
using BeersCheersVasis.Api.Models.LinkPreview;
using HtmlAgilityPack;

namespace BeersCheersVasis.Services.Implementation;

public sealed class LinkPreviewService : ILinkPreviewService
{
    private readonly IHttpClientFactory _httpClientFactory;

    // oEmbed endpoints for popular providers
    private static readonly Dictionary<string, string> OEmbedProviders = new(StringComparer.OrdinalIgnoreCase)
    {
        { "youtube.com", "https://www.youtube.com/oembed?url={0}&format=json" },
        { "youtu.be", "https://www.youtube.com/oembed?url={0}&format=json" },
        { "twitter.com", "https://publish.twitter.com/oembed?url={0}" },
        { "x.com", "https://publish.twitter.com/oembed?url={0}" },
        { "vimeo.com", "https://vimeo.com/api/oembed.json?url={0}" },
        { "instagram.com", "https://graph.facebook.com/v18.0/instagram_oembed?url={0}&access_token=client" },
    };

    public LinkPreviewService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<LinkPreviewResponse> GetPreviewAsync(string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(url, nameof(url));

        var uri = new Uri(url);
        var host = uri.Host.Replace("www.", "");

        // Try oEmbed first for known providers
        if (OEmbedProviders.TryGetValue(host, out var oEmbedEndpoint))
        {
            var oEmbedResult = await TryOEmbedAsync(url, oEmbedEndpoint, cancellationToken).ConfigureAwait(false);
            if (oEmbedResult is not null)
                return oEmbedResult;
        }

        // Fall back to OpenGraph
        return await FetchOpenGraphAsync(url, uri, cancellationToken).ConfigureAwait(false);
    }

    private async Task<LinkPreviewResponse?> TryOEmbedAsync(string url, string endpointTemplate, CancellationToken ct)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("LinkPreview");
            var endpoint = string.Format(endpointTemplate, Uri.EscapeDataString(url));
            var response = await client.GetAsync(endpoint, ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadFromJsonAsync<OEmbedResponse>(ct).ConfigureAwait(false);
            if (json is null) return null;

            return new LinkPreviewResponse
            {
                Url = url,
                Title = json.Title ?? string.Empty,
                Description = json.AuthorName ?? string.Empty,
                ImageUrl = json.ThumbnailUrl ?? string.Empty,
                SiteName = json.ProviderName ?? string.Empty,
                Type = json.Type == "video" ? "video" : "website",
                OEmbedHtml = json.Html
            };
        }
        catch
        {
            return null;
        }
    }

    private async Task<LinkPreviewResponse> FetchOpenGraphAsync(string url, Uri uri, CancellationToken ct)
    {
        var result = new LinkPreviewResponse { Url = url, SiteName = uri.Host.Replace("www.", "") };

        try
        {
            var client = _httpClientFactory.CreateClient("LinkPreview");
            var html = await client.GetStringAsync(url, ct).ConfigureAwait(false);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            result.Title = GetMetaContent(doc, "og:title")
                        ?? doc.DocumentNode.SelectSingleNode("//title")?.InnerText?.Trim()
                        ?? string.Empty;
            result.Description = GetMetaContent(doc, "og:description")
                              ?? GetMetaContent(doc, "description")
                              ?? string.Empty;
            result.ImageUrl = GetMetaContent(doc, "og:image") ?? string.Empty;
            result.SiteName = GetMetaContent(doc, "og:site_name") ?? result.SiteName;
            result.Type = GetMetaContent(doc, "og:type") ?? "website";
        }
        catch
        {
            // Return partial result on failure
        }

        return result;
    }

    private static string? GetMetaContent(HtmlDocument doc, string property)
    {
        // Try og: property
        var node = doc.DocumentNode.SelectSingleNode($"//meta[@property='{property}']");
        if (node is not null) return node.GetAttributeValue("content", null);

        // Try name attribute (for description, etc.)
        node = doc.DocumentNode.SelectSingleNode($"//meta[@name='{property}']");
        return node?.GetAttributeValue("content", null);
    }

    private sealed class OEmbedResponse
    {
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public string? ProviderName { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Html { get; set; }
        public string? Type { get; set; }
    }
}

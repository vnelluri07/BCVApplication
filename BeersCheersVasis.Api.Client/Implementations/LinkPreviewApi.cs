using BeersCheersVasis.Api.Models.LinkPreview;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class LinkPreviewApi : ILinkPreviewApi
{
    private readonly BcvHttpClient _httpClient;

    public LinkPreviewApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<LinkPreviewResponse> GetPreviewAsync(string url)
    {
        var encoded = Uri.EscapeDataString(url);
        var result = await _httpClient.GetFromJsonAsync<LinkPreviewResponse>($"LinkPreview/preview?url={encoded}").ConfigureAwait(false);
        return result ?? new LinkPreviewResponse { Url = url };
    }
}

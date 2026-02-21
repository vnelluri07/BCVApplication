using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace BeersCheersVasis.Services.Implementation;

public sealed class TurnstileService : ITurnstileService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _secretKey;

    public TurnstileService(IHttpClientFactory httpClientFactory, string secretKey)
    {
        _httpClientFactory = httpClientFactory;
        _secretKey = secretKey;
    }

    public async Task<bool> VerifyAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;

        var client = _httpClientFactory.CreateClient();
        var payload = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["secret"] = _secretKey,
            ["response"] = token
        });

        var response = await client.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", payload, cancellationToken);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<TurnstileResponse>(cancellationToken: cancellationToken);
        return result?.Success ?? false;
    }

    private sealed class TurnstileResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}

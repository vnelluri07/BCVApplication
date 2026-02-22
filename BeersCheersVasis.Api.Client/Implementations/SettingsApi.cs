using System.Text.Json;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class SettingsApi : ISettingsApi
{
    private readonly BcvHttpClient _httpClient;

    public SettingsApi(BcvHttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> GetAsync(string key)
    {
        var result = await _httpClient.GetFromJsonAsync<JsonElement>($"Settings/{key}");
        return result.GetProperty("value").GetString() ?? "";
    }

    public async Task SetAsync(string key, string value)
    {
        await _httpClient.PutAsJsonAsync<object, object>($"Settings/{key}", new { value });
    }
}

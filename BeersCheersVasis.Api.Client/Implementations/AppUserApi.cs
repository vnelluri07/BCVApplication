using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class AppUserApi : IAppUserApi
{
    private readonly BcvHttpClient _httpClient;

    public AppUserApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<AppUserResponse> GoogleAuthAsync(GoogleAuthRequest request)
    {
        return await _httpClient.PostAsJsonAsync<GoogleAuthRequest, AppUserResponse>("AppUser/GoogleAuth", request);
    }

    public async Task<AppUserResponse> CreateAnonymousAsync()
    {
        return await _httpClient.PostAsJsonAsync<object, AppUserResponse>("AppUser/Anonymous", new { });
    }

    public async Task<AppUserResponse?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AppUserResponse>($"AppUser/Get/{id}");
    }
}

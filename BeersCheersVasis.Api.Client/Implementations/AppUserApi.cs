using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class AppUserApi : IAppUserApi
{
    private readonly BcvHttpClient _httpClient;
    public AppUserApi(BcvHttpClient httpClient) => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<AppUserResponse> GoogleAuthAsync(GoogleAuthRequest request)
        => await _httpClient.PostAsJsonAsync<GoogleAuthRequest, AppUserResponse>("AppUser/GoogleAuth", request);

    public async Task<AppUserResponse> CreateAnonymousAsync()
        => await _httpClient.PostAsJsonAsync<object, AppUserResponse>("AppUser/Anonymous", new { });

    public async Task<AppUserResponse?> GetAsync(int id)
        => await _httpClient.GetFromJsonAsync<AppUserResponse>($"AppUser/Get/{id}");

    public async Task<IEnumerable<AppUserResponse>> GetAllAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<AppUserResponse>>("AppUser/GetAll")
           ?? [];

    public async Task SetRoleAsync(int id, string role)
        => await _httpClient.PutAsync($"AppUser/SetRole/{id}?role={Uri.EscapeDataString(role)}");

    public async Task ToggleActiveAsync(int id)
        => await _httpClient.PutAsync($"AppUser/ToggleActive/{id}");
}

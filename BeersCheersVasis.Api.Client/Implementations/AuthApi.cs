namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class AuthApi : IAuthApi
{
    private readonly BcvHttpClient _httpClient;

    public AuthApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<AuthLoginResponse> GoogleLoginAsync(string idToken, string? turnstileToken = null)
    {
        return await _httpClient.PostAsJsonAsync<object, AuthLoginResponse>("Auth/google",
            new { IdToken = idToken, TurnstileToken = turnstileToken });
    }

    public async Task<AuthLoginResponse> GoogleLoginWithCodeAsync(string code, string redirectUri)
    {
        return await _httpClient.PostAsJsonAsync<object, AuthLoginResponse>("Auth/google-code",
            new { Code = code, RedirectUri = redirectUri });
    }

    public async Task<bool> VerifyTurnstileAsync(string token)
    {
        var result = await _httpClient.PostAsJsonAsync<object, TurnstileResult>("Auth/verify-turnstile",
            new { Token = token });
        return result.Success;
    }

    private sealed class TurnstileResult { public bool Success { get; set; } }
}

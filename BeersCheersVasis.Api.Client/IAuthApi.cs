namespace BeersCheersVasis.Api.Client;

public interface IAuthApi
{
    Task<AuthLoginResponse> GoogleLoginAsync(string idToken, string? turnstileToken = null);
    Task<bool> VerifyTurnstileAsync(string token);
}

public sealed class AuthLoginResponse
{
    public string Token { get; set; } = "";
    public BeersCheersVasis.Api.Models.AppUser.AppUserResponse User { get; set; } = new();
}

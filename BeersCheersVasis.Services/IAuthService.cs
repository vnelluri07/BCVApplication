using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Services;

public interface IAuthService
{
    Task<AuthResponse> GoogleLoginAsync(string idToken, CancellationToken cancellationToken);
}

public sealed class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public AppUserResponse User { get; set; } = new();
}

using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Services;

public interface IAppUserService
{
    Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken);
    Task<AppUserResponse> GetOrCreateAnonymousUserAsync(CancellationToken cancellationToken);
    Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

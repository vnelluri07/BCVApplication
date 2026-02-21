using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Repository;

public interface IAppUserRepository
{
    Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken);
    Task<AppUserResponse> CreateAnonymousUserAsync(string displayName, CancellationToken cancellationToken);
    Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Repository;

public interface IAppUserRepository
{
    Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken);
    Task<AppUserResponse> CreateAnonymousUserAsync(string displayName, CancellationToken cancellationToken);
    Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task SetRoleAsync(int id, string role, CancellationToken cancellationToken);
    Task<IEnumerable<AppUserResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task ToggleActiveAsync(int id, CancellationToken cancellationToken);
}

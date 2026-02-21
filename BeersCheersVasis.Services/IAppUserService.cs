using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Services;

public interface IAppUserService
{
    Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken);
    Task<AppUserResponse> GetOrCreateAnonymousUserAsync(CancellationToken cancellationToken);
    Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<AppUserResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task SetRoleAsync(int id, string role, CancellationToken cancellationToken);
    Task ToggleActiveAsync(int id, CancellationToken cancellationToken);
}

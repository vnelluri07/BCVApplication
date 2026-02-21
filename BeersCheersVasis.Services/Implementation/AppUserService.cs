using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAnonymousNameGenerator _nameGenerator;

    public AppUserService(IAppUserRepository appUserRepository, IAnonymousNameGenerator nameGenerator)
    {
        _appUserRepository = appUserRepository;
        _nameGenerator = nameGenerator;
    }

    public async Task<AppUserResponse> GetOrCreateGoogleUserAsync(GoogleAuthRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _appUserRepository.GetOrCreateGoogleUserAsync(request, cancellationToken);
    }

    public async Task<AppUserResponse> GetOrCreateAnonymousUserAsync(CancellationToken cancellationToken)
    {
        var displayName = _nameGenerator.Generate();
        return await _appUserRepository.CreateAnonymousUserAsync(displayName, cancellationToken);
    }

    public async Task<AppUserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _appUserRepository.GetByIdAsync(id, cancellationToken);
    }
}

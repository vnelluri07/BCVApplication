using BeersCheersVasis.Api.Models.AppUser;

namespace BeersCheersVasis.Api.Client;

public interface IAppUserApi
{
    Task<AppUserResponse> GoogleAuthAsync(GoogleAuthRequest request);
    Task<AppUserResponse> CreateAnonymousAsync();
    Task<AppUserResponse?> GetAsync(int id);
}

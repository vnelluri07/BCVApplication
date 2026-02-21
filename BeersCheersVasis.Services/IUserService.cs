using BeersCheersVasis.Api.Models.User;

namespace BeersCheersVasis.Services;

public interface IUserService
{
    #region Users
    Task<IEnumerable<UserResponse>> GetUsers(CancellationToken cancellationToken);

    #endregion
}

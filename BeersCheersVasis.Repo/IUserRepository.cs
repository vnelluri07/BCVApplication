using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Api.Models.User;

namespace BeersCheersVasis.Repo;
public interface IUserRepository
{
    #region Users
    Task<IEnumerable<UserResponse>> GetUserAsync(CancellationToken cancellationToken);
    #endregion

}

using BeersCheersVasis.Api.Models.User;
using BeersCheersVasis.Repo;

namespace BeersCheersVasis.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> GetUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserAsync(cancellationToken);
    }
}


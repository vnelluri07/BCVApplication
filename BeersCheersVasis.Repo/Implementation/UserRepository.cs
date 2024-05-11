using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.User;
using BeersCheersVasis.Repo;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;
public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public UserRepository(IUnitOfWork dbContext)
    {
        _unitOfWork = dbContext;
    }

    public async Task<IEnumerable<UserResponse>> GetUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync(cancellationToken);

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                IsActive = u.IsActive
            });
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}


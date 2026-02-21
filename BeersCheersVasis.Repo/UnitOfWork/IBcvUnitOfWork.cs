using BeersCheersVasis.Repo;

namespace BeersCheersVasis.Repository.UnitOfWork;

public class IBcvUnitOfWork
{
    public IUserRepository UserRepository { get; }
}

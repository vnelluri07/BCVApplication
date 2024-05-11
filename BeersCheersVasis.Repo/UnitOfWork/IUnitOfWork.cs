using BeersCheersAndVasis.UI.Data.Context;
namespace BeersCheersVasis.Repo.UnitOfWork;

public interface IUnitOfWork
{
    public IdbContext DbContext { get; }
}

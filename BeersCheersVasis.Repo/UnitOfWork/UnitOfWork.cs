using BeersCheersAndVasis.UI.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
namespace BeersCheersVasis.Repo.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{

    private readonly IUnitOfWork _unitOfWork;
    //public IdbContext _dbContext => _unitOfWork.DbContext;

    public IdbContext DbContext => _unitOfWork.DbContext;

    public UnitOfWork(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task<IDbContextTransaction> GetDbTransactionAsync()
    //{
    //    return await _dbContext.Database.BeginTransactionAsync();
    //}

    //public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    //{
    //    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    //}

    //public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    //{
    //    await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    //}
}

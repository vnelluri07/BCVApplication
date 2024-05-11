using BeersCheersVasis.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BeersCheersAndVasis.UI.Data.Context;

public interface IdbContext
{
    DbSet<User> Users { get; }
    DbSet<Script> Script { get; }

    //TODO:will add this later in UnitOfWork
    //DatabaseFacade Database { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

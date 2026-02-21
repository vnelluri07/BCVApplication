using BeersCheersVasis.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BeersCheersAndVasis.UI.Data.Context;

public interface IdbContext
{
    DbSet<User> Users { get; }
    DbSet<Script> Script { get; }
    DbSet<Category> Categories { get; }
    DbSet<Comment> Comments { get; }
    DbSet<AppUser> AppUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

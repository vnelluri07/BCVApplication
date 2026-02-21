using BeersCheersVasis.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeersCheersAndVasis.UI.Data.Context.Implementation;

public class dbContext : DbContext, IdbContext
{
    public dbContext(DbContextOptions<dbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Script> Script => Set<Script>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

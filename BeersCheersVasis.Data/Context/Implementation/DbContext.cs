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

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.Script)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.ScriptId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Script>(entity =>
        {
            entity.HasOne(s => s.Category)
                .WithMany(c => c.Scripts)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Script> Script => Set<Script>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

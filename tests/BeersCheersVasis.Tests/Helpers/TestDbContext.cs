using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Tests.Helpers;

public class TestDbContext : DbContext, IdbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Script> Script { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<AppUser> AppUsers { get; set; } = null!;

    public static TestDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;
        var ctx = new TestDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }
}

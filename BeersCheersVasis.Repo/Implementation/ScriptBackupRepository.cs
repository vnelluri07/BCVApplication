using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class ScriptBackupRepository(IdbContext dbContext) : IScriptBackupRepository
{
    private readonly IdbContext _dbContext = dbContext;

    public async Task<ScriptBackup?> GetLatestAsync(int scriptId, string provider, CancellationToken cancellationToken)
        => await _dbContext.ScriptBackups
            .Where(b => b.ScriptId == scriptId && b.Provider == provider)
            .OrderByDescending(b => b.BackedUpAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<ScriptBackup>> GetByScriptIdAsync(int scriptId, CancellationToken cancellationToken)
        => await _dbContext.ScriptBackups
            .Where(b => b.ScriptId == scriptId)
            .OrderByDescending(b => b.BackedUpAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ScriptBackup backup, CancellationToken cancellationToken)
    {
        _dbContext.ScriptBackups.Add(backup);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

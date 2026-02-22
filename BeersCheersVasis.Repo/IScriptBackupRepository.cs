using BeersCheersVasis.Data.Entities;

namespace BeersCheersVasis.Repository;

public interface IScriptBackupRepository
{
    Task<ScriptBackup?> GetLatestAsync(int scriptId, string provider, CancellationToken cancellationToken);
    Task<IEnumerable<ScriptBackup>> GetByScriptIdAsync(int scriptId, CancellationToken cancellationToken);
    Task AddAsync(ScriptBackup backup, CancellationToken cancellationToken);
}

using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Repository;

public interface IScriptRepository
{
    Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken);
    Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken);
    Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken);
}

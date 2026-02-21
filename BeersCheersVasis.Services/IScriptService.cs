using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Services;

public interface IScriptService
{
    Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken);
    Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken);
    Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken);
    Task<ScriptResponse> UpdateScriptAsync(UpdateScriptRequest request, CancellationToken cancellationToken);
}

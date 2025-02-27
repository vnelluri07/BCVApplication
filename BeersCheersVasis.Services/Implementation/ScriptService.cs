using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Repository;
using ArgumentNullException = System.ArgumentNullException;

namespace BeersCheersVasis.Services.Implementation;

public sealed class ScriptService : IScriptService
{
    private readonly IScriptRepository _scriptRepository;

    public ScriptService(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    public async Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken)
    {
        return await _scriptRepository.GetScriptsAsync(cancellationToken);
    }

    public Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken)
    {
        if(id == 0)
            throw new ArgumentNullException(nameof(GetScriptAsync));

        return _scriptRepository.GetScriptAsync(id, cancellationToken);
    }

    public async Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(CreateScriptAsync));

        var response = await _scriptRepository.CreateScriptAsync(request, cancellationToken);
        return response;
    }
}

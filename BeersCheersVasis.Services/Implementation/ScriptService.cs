using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Repo;
using BeersCheersVasis.Repository;
using Microsoft.Identity.Client;

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
        try
        {
            return await _scriptRepository.GetScriptsAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }

    public async Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        //TODO:what?
        if (request == null)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(CreateScriptAsync));
        }

        var response = await _scriptRepository.CreateScriptAsync(request, cancellationToken);
        return response;
    }
}

using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class ScriptService : IScriptService
{
    private readonly IScriptRepository _scriptRepository;

    public ScriptService(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    public async Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken)
        => await _scriptRepository.GetScriptsAsync(cancellationToken);

    public async Task<IEnumerable<ScriptResponse>> GetPublishedScriptsAsync(CancellationToken cancellationToken)
        => await _scriptRepository.GetPublishedScriptsAsync(cancellationToken);

    public async Task<IEnumerable<ScriptResponse>> GetPublishedByCategoryAsync(int categoryId, CancellationToken cancellationToken)
        => await _scriptRepository.GetPublishedByCategoryAsync(categoryId, cancellationToken);

    public Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken)
    {
        if (id == 0) throw new ArgumentNullException(nameof(id));
        return _scriptRepository.GetScriptAsync(id, cancellationToken);
    }

    public async Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _scriptRepository.CreateScriptAsync(request, cancellationToken);
    }

    public async Task<ScriptResponse> UpdateScriptAsync(UpdateScriptRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _scriptRepository.UpdateScriptAsync(request, cancellationToken);
    }

    public async Task PublishScriptAsync(int id, CancellationToken cancellationToken)
        => await _scriptRepository.PublishScriptAsync(id, cancellationToken);

    public async Task UnpublishScriptAsync(int id, CancellationToken cancellationToken)
        => await _scriptRepository.UnpublishScriptAsync(id, cancellationToken);

    public async Task SoftDeleteScriptAsync(int id, CancellationToken cancellationToken)
        => await _scriptRepository.SoftDeleteScriptAsync(id, cancellationToken);

    public async Task SetCategoryAsync(int id, int categoryId, CancellationToken cancellationToken)
        => await _scriptRepository.SetCategoryAsync(id, categoryId, cancellationToken);
}

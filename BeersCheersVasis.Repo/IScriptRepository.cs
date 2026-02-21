using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Repository;

public interface IScriptRepository
{
    Task<IEnumerable<ScriptResponse>> GetScriptsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ScriptResponse>> GetPublishedScriptsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ScriptResponse>> GetPublishedByCategoryAsync(int categoryId, CancellationToken cancellationToken);
    Task<ScriptResponse> GetScriptAsync(int id, CancellationToken cancellationToken);
    Task<ScriptResponse> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken);
    Task<ScriptResponse> UpdateScriptAsync(UpdateScriptRequest request, CancellationToken cancellationToken);
    Task PublishScriptAsync(int id, CancellationToken cancellationToken);
    Task<int> PublishAllScriptsAsync(CancellationToken cancellationToken);
    Task UnpublishScriptAsync(int id, CancellationToken cancellationToken);
    Task SoftDeleteScriptAsync(int id, CancellationToken cancellationToken);
    Task SetCategoryAsync(int id, int categoryId, CancellationToken cancellationToken);
}

using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Api.Client;

public interface IScriptApi
{
    Task<IEnumerable<ScriptResponse>> ListAsync();
    Task<IEnumerable<ScriptResponse>> GetPublishedAsync();
    Task<IEnumerable<ScriptResponse>> GetPublishedAsync(int categoryId);
    Task<ScriptResponse> GetAsync(int id);
    Task<ScriptResponse> CreateAsync(CreateScriptRequest request);
    Task<ScriptResponse> UpdateAsync(UpdateScriptRequest request);
    Task PublishAsync(int id);
    Task<int> PublishAllAsync();
    Task UnpublishAsync(int id);
    Task SoftDeleteAsync(int id);
    Task SetCategoryAsync(int id, int categoryId);
}

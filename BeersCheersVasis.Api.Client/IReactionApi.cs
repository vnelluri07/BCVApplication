using BeersCheersVasis.Api.Models.Reaction;

namespace BeersCheersVasis.Api.Client;

public interface IReactionApi
{
    Task<ReactionCountsResponse> ReactAsync(ReactRequest request);
    Task<ReactionCountsResponse> GetScriptCountsAsync(int scriptId, string? voterKey);
    Task<IEnumerable<ReactionCountsResponse>> GetBulkScriptCountsAsync(int[] scriptIds, string? voterKey);
    Task<IEnumerable<ReactionCountsResponse>> GetCommentCountsAsync(int[] commentIds, string? voterKey);
}

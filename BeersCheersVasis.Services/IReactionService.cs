using BeersCheersVasis.Api.Models.Reaction;

namespace BeersCheersVasis.Services;

public interface IReactionService
{
    Task<ReactionCountsResponse> ReactAsync(ReactRequest request, CancellationToken cancellationToken);
    Task RemoveReactionAsync(int? scriptId, int? commentId, string voterKey, CancellationToken cancellationToken);
    Task<ReactionCountsResponse> GetCountsAsync(int? scriptId, int? commentId, string? voterKey, CancellationToken cancellationToken);
    Task<IEnumerable<ReactionCountsResponse>> GetBulkCountsAsync(int[] scriptIds, string? voterKey, CancellationToken cancellationToken);
    Task<IEnumerable<ReactionCountsResponse>> GetCommentCountsAsync(int[] commentIds, string? voterKey, CancellationToken cancellationToken);
}

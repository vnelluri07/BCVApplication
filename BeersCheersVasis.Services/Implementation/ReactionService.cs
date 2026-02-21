using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class ReactionService : IReactionService
{
    private readonly IReactionRepository _repo;
    public ReactionService(IReactionRepository repo) => _repo = repo;

    public Task<ReactionCountsResponse> ReactAsync(ReactRequest request, CancellationToken cancellationToken)
        => _repo.ReactAsync(request, cancellationToken);

    public Task RemoveReactionAsync(int? scriptId, int? commentId, string voterKey, CancellationToken cancellationToken)
        => _repo.RemoveReactionAsync(scriptId, commentId, voterKey, cancellationToken);

    public Task<ReactionCountsResponse> GetCountsAsync(int? scriptId, int? commentId, string? voterKey, CancellationToken cancellationToken)
        => _repo.GetCountsAsync(scriptId, commentId, voterKey, cancellationToken);

    public Task<IEnumerable<ReactionCountsResponse>> GetBulkCountsAsync(int[] scriptIds, string? voterKey, CancellationToken cancellationToken)
        => _repo.GetBulkCountsAsync(scriptIds, voterKey, cancellationToken);

    public Task<IEnumerable<ReactionCountsResponse>> GetCommentCountsAsync(int[] commentIds, string? voterKey, CancellationToken cancellationToken)
        => _repo.GetCommentCountsAsync(commentIds, voterKey, cancellationToken);
}

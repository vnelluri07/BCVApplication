using BeersCheersVasis.Api.Models.Comment;

namespace BeersCheersVasis.Repository;

public interface ICommentRepository
{
    Task<IEnumerable<CommentResponse>> GetCommentsByScriptAsync(int scriptId, CancellationToken cancellationToken);
    Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, int appUserId, CancellationToken cancellationToken);
    Task DeleteCommentAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CommentResponse>> SearchCommentsAsync(int scriptId, string keyword, CancellationToken cancellationToken);
}

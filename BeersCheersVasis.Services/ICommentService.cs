using BeersCheersVasis.Api.Models.Comment;

namespace BeersCheersVasis.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetCommentsByScriptAsync(int scriptId, CancellationToken cancellationToken);
    Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, CancellationToken cancellationToken);
    Task DeleteCommentAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CommentResponse>> SearchCommentsAsync(int scriptId, string keyword, CancellationToken cancellationToken);
}

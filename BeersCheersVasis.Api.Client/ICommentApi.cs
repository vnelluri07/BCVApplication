using BeersCheersVasis.Api.Models.Comment;

namespace BeersCheersVasis.Api.Client;

public interface ICommentApi
{
    Task<IEnumerable<CommentResponse>> GetAllAsync();
    Task<IEnumerable<CommentResponse>> GetByScriptAsync(int scriptId);
    Task<CommentResponse> CreateAsync(CreateCommentRequest request);
    Task DeleteAsync(int id);
    Task<IEnumerable<CommentResponse>> SearchAsync(int scriptId, string keyword);
}

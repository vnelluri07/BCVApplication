using BeersCheersVasis.Api.Models.Comment;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAnonymousNameGenerator _nameGenerator;

    public CommentService(ICommentRepository commentRepository, IAppUserRepository appUserRepository, IAnonymousNameGenerator nameGenerator)
    {
        _commentRepository = commentRepository;
        _appUserRepository = appUserRepository;
        _nameGenerator = nameGenerator;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsByScriptAsync(int scriptId, CancellationToken cancellationToken)
    {
        return await _commentRepository.GetCommentsByScriptAsync(scriptId, cancellationToken);
    }

    public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        int appUserId;

        if (request.AppUserId.HasValue && request.AppUserId.Value > 0)
        {
            appUserId = request.AppUserId.Value;
        }
        else
        {
            var displayName = request.AnonymousDisplayName ?? _nameGenerator.Generate();
            var anonUser = await _appUserRepository.CreateAnonymousUserAsync(displayName, cancellationToken);
            appUserId = anonUser.Id;
        }

        return await _commentRepository.CreateCommentAsync(request, appUserId, cancellationToken);
    }

    public async Task DeleteCommentAsync(int id, CancellationToken cancellationToken)
    {
        await _commentRepository.DeleteCommentAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<CommentResponse>> SearchCommentsAsync(int scriptId, string keyword, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("Keyword is required.", nameof(keyword));
        return await _commentRepository.SearchCommentsAsync(scriptId, keyword, cancellationToken);
    }
}

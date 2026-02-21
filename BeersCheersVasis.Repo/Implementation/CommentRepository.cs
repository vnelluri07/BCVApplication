using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.Comment;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class CommentRepository : ICommentRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public CommentRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsByScriptAsync(int scriptId, CancellationToken cancellationToken)
    {
        var comments = await _dbContext.Comments
            .Include(c => c.AppUser)
            .Where(c => c.ScriptId == scriptId)
            .OrderBy(c => c.CreatedDate)
            .ToListAsync(cancellationToken);

        var topLevel = comments.Where(c => c.ParentCommentId == null);
        return topLevel.Select(c => MapToTree(c, comments)).ToList();
    }

    public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, int appUserId, CancellationToken cancellationToken)
    {
        var entity = new Comment
        {
            ScriptId = request.ScriptId,
            AppUserId = appUserId,
            ParentCommentId = request.ParentCommentId,
            Content = request.Content,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.Comments.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == appUserId, cancellationToken);

        return new CommentResponse
        {
            Id = entity.Id,
            ScriptId = entity.ScriptId,
            AppUserId = entity.AppUserId,
            AuthorDisplayName = user?.DisplayName ?? "Unknown",
            AuthorAvatarUrl = user?.AvatarUrl,
            IsAnonymous = user?.IsAnonymous ?? true,
            ParentCommentId = entity.ParentCommentId,
            Content = entity.Content,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate
        };
    }

    public async Task DeleteCommentAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new ArgumentException($"Comment with ID '{id}' not found.");

        entity.IsDeleted = true;
        entity.Content = "[deleted]";
        entity.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CommentResponse>> SearchCommentsAsync(int scriptId, string keyword, CancellationToken cancellationToken)
    {
        var comments = await _dbContext.Comments
            .Include(c => c.AppUser)
            .Where(c => c.ScriptId == scriptId && !c.IsDeleted && c.Content.Contains(keyword))
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => new CommentResponse
            {
                Id = c.Id,
                ScriptId = c.ScriptId,
                AppUserId = c.AppUserId,
                AuthorDisplayName = c.AppUser.DisplayName,
                AuthorAvatarUrl = c.AppUser.AvatarUrl,
                IsAnonymous = c.AppUser.IsAnonymous,
                ParentCommentId = c.ParentCommentId,
                Content = c.Content,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate
            })
            .ToListAsync(cancellationToken);

        return comments;
    }

    private static CommentResponse MapToTree(Comment comment, IEnumerable<Comment> allComments)
    {
        var response = new CommentResponse
        {
            Id = comment.Id,
            ScriptId = comment.ScriptId,
            AppUserId = comment.AppUserId,
            AuthorDisplayName = comment.AppUser?.DisplayName ?? "Unknown",
            AuthorAvatarUrl = comment.AppUser?.AvatarUrl,
            IsAnonymous = comment.AppUser?.IsAnonymous ?? true,
            ParentCommentId = comment.ParentCommentId,
            Content = comment.IsDeleted ? "[deleted]" : comment.Content,
            IsDeleted = comment.IsDeleted,
            CreatedDate = comment.CreatedDate,
            ModifiedDate = comment.ModifiedDate,
            Replies = allComments
                .Where(c => c.ParentCommentId == comment.Id)
                .Select(c => MapToTree(c, allComments))
                .ToList()
        };

        return response;
    }
}

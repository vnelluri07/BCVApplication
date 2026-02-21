using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class ReactionRepository : IReactionRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _db => _unitOfWork.DbContext;

    public ReactionRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ReactionCountsResponse> ReactAsync(ReactRequest request, CancellationToken cancellationToken)
    {
        var existing = await _db.Reactions.FirstOrDefaultAsync(r =>
            r.VoterKey == request.VoterKey &&
            r.ScriptId == request.ScriptId &&
            r.CommentId == request.CommentId, cancellationToken);

        if (existing is not null)
        {
            if (existing.ReactionType == (int)request.ReactionType)
            {
                // Same reaction = toggle off
                _db.Reactions.Remove(existing);
                await _db.SaveChangesAsync(cancellationToken);
                return await GetCountsAsync(request.ScriptId, request.CommentId, request.VoterKey, cancellationToken);
            }
            existing.ReactionType = (int)request.ReactionType;
            existing.CreatedDate = DateTime.UtcNow;
        }
        else
        {
            _db.Reactions.Add(new Reaction
            {
                ScriptId = request.ScriptId,
                CommentId = request.CommentId,
                VoterKey = request.VoterKey,
                ReactionType = (int)request.ReactionType,
                CreatedDate = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync(cancellationToken);
        return await GetCountsAsync(request.ScriptId, request.CommentId, request.VoterKey, cancellationToken);
    }

    public async Task RemoveReactionAsync(int? scriptId, int? commentId, string voterKey, CancellationToken cancellationToken)
    {
        var existing = await _db.Reactions.FirstOrDefaultAsync(r =>
            r.VoterKey == voterKey && r.ScriptId == scriptId && r.CommentId == commentId, cancellationToken);
        if (existing is not null)
        {
            _db.Reactions.Remove(existing);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<ReactionCountsResponse> GetCountsAsync(int? scriptId, int? commentId, string? voterKey, CancellationToken cancellationToken)
    {
        var query = _db.Reactions.Where(r => r.ScriptId == scriptId && r.CommentId == commentId);
        var all = await query.ToListAsync(cancellationToken);

        return new ReactionCountsResponse
        {
            ScriptId = scriptId,
            CommentId = commentId,
            SuperLikes = all.Count(r => r.ReactionType == (int)ReactionType.SuperLike),
            Likes = all.Count(r => r.ReactionType == (int)ReactionType.Like),
            Dislikes = all.Count(r => r.ReactionType == (int)ReactionType.Dislike),
            UserReaction = voterKey is null ? null :
                all.FirstOrDefault(r => r.VoterKey == voterKey) is { } ur ? (ReactionType)ur.ReactionType : null
        };
    }

    public async Task<IEnumerable<ReactionCountsResponse>> GetBulkCountsAsync(int[] scriptIds, string? voterKey, CancellationToken cancellationToken)
    {
        var reactions = await _db.Reactions
            .Where(r => r.ScriptId != null && scriptIds.Contains(r.ScriptId.Value))
            .ToListAsync(cancellationToken);

        return scriptIds.Select(sid =>
        {
            var group = reactions.Where(r => r.ScriptId == sid).ToList();
            return new ReactionCountsResponse
            {
                ScriptId = sid,
                SuperLikes = group.Count(r => r.ReactionType == (int)ReactionType.SuperLike),
                Likes = group.Count(r => r.ReactionType == (int)ReactionType.Like),
                Dislikes = group.Count(r => r.ReactionType == (int)ReactionType.Dislike),
                UserReaction = voterKey is null ? null :
                    group.FirstOrDefault(r => r.VoterKey == voterKey) is { } ur ? (ReactionType)ur.ReactionType : null
            };
        });
    }

    public async Task<IEnumerable<ReactionCountsResponse>> GetCommentCountsAsync(int[] commentIds, string? voterKey, CancellationToken cancellationToken)
    {
        var reactions = await _db.Reactions
            .Where(r => r.CommentId != null && commentIds.Contains(r.CommentId.Value))
            .ToListAsync(cancellationToken);

        return commentIds.Select(cid =>
        {
            var group = reactions.Where(r => r.CommentId == cid).ToList();
            return new ReactionCountsResponse
            {
                CommentId = cid,
                SuperLikes = group.Count(r => r.ReactionType == (int)ReactionType.SuperLike),
                Likes = group.Count(r => r.ReactionType == (int)ReactionType.Like),
                Dislikes = group.Count(r => r.ReactionType == (int)ReactionType.Dislike),
                UserReaction = voterKey is null ? null :
                    group.FirstOrDefault(r => r.VoterKey == voterKey) is { } ur ? (ReactionType)ur.ReactionType : null
            };
        });
    }
}

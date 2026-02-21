namespace BeersCheersVasis.Api.Models.Reaction;

public enum ReactionType
{
    SuperLike = 1,
    Like = 2,
    Dislike = 3
}

public sealed class ReactRequest
{
    public int? ScriptId { get; set; }
    public int? CommentId { get; set; }
    public string VoterKey { get; set; } = string.Empty;
    public ReactionType ReactionType { get; set; }
}

public sealed class ReactionCountsResponse
{
    public int? ScriptId { get; set; }
    public int? CommentId { get; set; }
    public int SuperLikes { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public ReactionType? UserReaction { get; set; }
}

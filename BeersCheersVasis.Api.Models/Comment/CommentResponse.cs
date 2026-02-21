namespace BeersCheersVasis.Api.Models.Comment;

public sealed class CommentResponse
{
    public int Id { get; set; }
    public int ScriptId { get; set; }
    public int AppUserId { get; set; }
    public string AuthorDisplayName { get; set; }
    public string? AuthorAvatarUrl { get; set; }
    public bool IsAnonymous { get; set; }
    public int? ParentCommentId { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<CommentResponse> Replies { get; set; } = new();
    public string? ScriptTitle { get; set; }
}

namespace BeersCheersVasis.Api.Models.Script;

public sealed class ScriptResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? PublishedDate { get; set; }
    public DateTime? ScheduledPublishDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int ModifiedBy { get; set; }
    public int CommentCount { get; set; }
}

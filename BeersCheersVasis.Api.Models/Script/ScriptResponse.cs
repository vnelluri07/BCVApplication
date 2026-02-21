namespace BeersCheersVasis.Api.Models.Script;

public sealed class ScriptResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive{ get; set; }
    public DateTime? CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int ModifiedBy { get; set; }
}

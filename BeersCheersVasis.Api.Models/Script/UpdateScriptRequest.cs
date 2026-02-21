namespace BeersCheersVasis.Api.Models.Script;

public sealed class UpdateScriptRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int ModifiedBy { get; set; }
}

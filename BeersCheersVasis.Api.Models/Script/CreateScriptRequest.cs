namespace BeersCheersVasis.Api.Models.Script;

public sealed class CreateScriptRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CreatedBy { get; set; }
}

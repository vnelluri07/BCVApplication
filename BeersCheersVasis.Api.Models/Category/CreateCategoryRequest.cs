namespace BeersCheersVasis.Api.Models.Category;

public sealed class CreateCategoryRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
}

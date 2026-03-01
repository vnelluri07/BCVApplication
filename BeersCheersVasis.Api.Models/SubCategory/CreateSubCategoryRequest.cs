namespace BeersCheersVasis.Api.Models.SubCategory;

public sealed class CreateSubCategoryRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
}

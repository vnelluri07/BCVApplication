namespace BeersCheersVasis.Api.Models.SubCategory;

public sealed class SubCategoryResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public int ScriptCount { get; set; }
}

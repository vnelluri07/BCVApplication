namespace BeersCheersVasis.Api.Models.AppUser;

public sealed class AppUserResponse
{
    public int Id { get; set; }
    public string DisplayName { get; set; }
    public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; }
    public bool IsAnonymous { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
}

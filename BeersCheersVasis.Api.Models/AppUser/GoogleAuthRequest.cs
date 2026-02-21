namespace BeersCheersVasis.Api.Models.AppUser;

public sealed class GoogleAuthRequest
{
    public string GoogleId { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
}

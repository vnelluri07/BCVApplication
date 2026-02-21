namespace BeersCheersVasis.Api.Models.User;
public sealed class UserResponse
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? IsActive { get; set; }
    public bool? PasswordResetRequired { get; set; }
}


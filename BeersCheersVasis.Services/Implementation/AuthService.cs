using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Repository;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;

namespace BeersCheersVasis.Services.Implementation;

public sealed class AuthService : IAuthService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly string _googleClientId;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly HashSet<string> _adminEmails;

    public AuthService(
        IAppUserRepository appUserRepository,
        string googleClientId,
        string jwtKey,
        string jwtIssuer,
        string jwtAudience,
        IEnumerable<string>? adminEmails = null)
    {
        _appUserRepository = appUserRepository;
        _googleClientId = googleClientId;
        _jwtKey = jwtKey;
        _jwtIssuer = jwtIssuer;
        _jwtAudience = jwtAudience;
        _adminEmails = new HashSet<string>(
            adminEmails ?? [], StringComparer.OrdinalIgnoreCase);
    }

    public async Task<AuthResponse> GoogleLoginAsync(string idToken, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_googleClientId]
        });

        var user = await _appUserRepository.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = payload.Subject,
            Email = payload.Email,
            DisplayName = payload.Name,
            AvatarUrl = payload.Picture
        }, cancellationToken);

        // Auto-promote configured admin emails
        if (_adminEmails.Contains(payload.Email) && user.Role != "Admin")
        {
            await _appUserRepository.SetRoleAsync(user.Id, "Admin", cancellationToken);
            user.Role = "Admin";
        }

        var jwt = GenerateJwt(user);

        return new AuthResponse { Token = jwt, User = user };
    }

    private string GenerateJwt(AppUserResponse user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

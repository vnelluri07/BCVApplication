using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Repository;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;

namespace BeersCheersVasis.Services.Implementation;

public sealed class AuthService : IAuthService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly string _googleClientId;
    private readonly string _googleClientSecret;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly HashSet<string> _adminEmails;
    private static readonly HttpClient _http = new();

    public AuthService(
        IAppUserRepository appUserRepository,
        string googleClientId,
        string googleClientSecret,
        string jwtKey,
        string jwtIssuer,
        string jwtAudience,
        IEnumerable<string>? adminEmails = null)
    {
        _appUserRepository = appUserRepository;
        _googleClientId = googleClientId;
        _googleClientSecret = googleClientSecret;
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

    public async Task<AuthResponse> GoogleLoginWithCodeAsync(string code, string redirectUri, CancellationToken cancellationToken)
    {
        // Exchange auth code for tokens
        var tokenResponse = await _http.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _googleClientId,
                ["client_secret"] = _googleClientSecret,
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code"
            }), cancellationToken);

        tokenResponse.EnsureSuccessStatusCode();
        var tokens = await tokenResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to parse Google token response.");

        // Validate the ID token and proceed same as GSI flow
        return await GoogleLoginAsync(tokens.IdToken, cancellationToken);
    }

    private sealed class GoogleTokenResponse
    {
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = "";
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

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BeersCheersVasis.API.Internal;

public interface IApiAuthService
{
    Task ValidateUserClaimsAsync(TokenValidatedContext context, CancellationToken cancellationToken);
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BeersCheersVasis.API.Internal;
public class ApiAuthService : IApiAuthService
{
    public Task ValidateUserClaimsAsync(TokenValidatedContext context, CancellationToken cancellationToken)
    {
        var principal = context.Principal
                        ?? throw new UnauthorizedAccessException("Invalid user claims");

        if (!principal.HasClaim(clm => clm.Type == ClaimTypes.NameIdentifier))
        {
            throw new UnauthorizedAccessException("Invalid user claims");
        }

        return Task.CompletedTask;
    }
}
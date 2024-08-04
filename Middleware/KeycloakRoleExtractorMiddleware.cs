using System.Security.Claims;
using Newtonsoft.Json;
using TJTech.APIs.ChatGTP.Data.Contracts.Keycloak;

namespace TJTech.APIs.ChatGTP.Middleware;

public class KeycloakRoleExtractorMiddleware(ILogger<KeycloakRoleExtractorMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var user = context.User;
        bool successful = !((user?.Identity is null || !user.Claims.Any()) || !user.Identity.IsAuthenticated);
        if (successful)
        {
            var resourceAccessString = user.Claims?.FirstOrDefault(c => c.Type == "resource_access")?.Value;
            var realmAccessString = user.Claims?.FirstOrDefault(c => c.Type == "realm_access")?.Value;
            successful = !string.IsNullOrEmpty(resourceAccessString) 
                         && !string.IsNullOrEmpty(realmAccessString);

            if (successful)
            {
                try
                {
                    var resourceAccess = JsonConvert.DeserializeObject<Dictionary<string, UserRoles>>(resourceAccessString);
                    var realmAccess = JsonConvert.DeserializeObject<UserRoles>(realmAccessString);
                    successful = resourceAccess is not null && realmAccess is not null;
                    if (successful)
                    {
                        foreach (var (key, value) in resourceAccess)
                        {
                            foreach (var role in value.Roles)
                            {
                                user
                                    .Identities
                                    .FirstOrDefault()
                                    ?.AddClaim(new Claim(ClaimTypes.Role, $"{key}:{role}"));    
                            }
                        }
                        foreach (var role in realmAccess.Roles)
                        {
                            user
                                .Identities
                                .FirstOrDefault()
                                ?.AddClaim(new Claim(ClaimTypes.Role, role));    
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("{e}", e);
                }
            }
        }
        await next.Invoke(context);
    }
}
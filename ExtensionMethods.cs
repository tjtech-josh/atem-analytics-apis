using System.Security.Claims;

namespace TJTech.APIs.ChatGTP;

public static class ExtensionMethods
{
    public static string? GetUerId(this ClaimsPrincipal user)
        => user?
            .Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
            .Value;
    
    public static string? GetFirstName(this ClaimsPrincipal user)
        => user?
            .Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?
            .Value;
    
    public static string? GetLastName(this ClaimsPrincipal user)
        => user?
            .Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?
            .Value;

    public static string GetFullName(this ClaimsPrincipal user)
        => $"{user.GetFirstName()} {user.GetLastName()}";
}
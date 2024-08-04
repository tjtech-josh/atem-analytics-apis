using Newtonsoft.Json;

namespace TJTech.APIs.ChatGTP.Data.Contracts.Keycloak;

public class UserRoles
{
    [JsonProperty("roles")]
    public List<string> Roles { get; set; }
}
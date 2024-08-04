using Newtonsoft.Json;

namespace TJTech.APIs.ChatGTP.Data.Contracts.Keycloak;

public class Resource
{
    [JsonProperty("resource_access")]
    public Dictionary<string, UserRoles> ResourceAccess { get; set; }
}
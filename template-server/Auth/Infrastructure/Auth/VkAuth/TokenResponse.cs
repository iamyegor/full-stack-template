using Newtonsoft.Json;

namespace Infrastructure.Auth.VkAuth;

public class TokenResponse
{
    [JsonProperty("user_id")]
    public string UserId { get; set; } = null!;
}

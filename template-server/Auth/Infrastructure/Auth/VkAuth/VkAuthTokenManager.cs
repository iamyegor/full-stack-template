using Domain.DomainErrors;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using XResults;

namespace Infrastructure.Auth.VkAuth;

public class VkAuthTokenManager
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public VkAuthTokenManager(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<Result<string, Error>> GetVkUserId(string code, string vkDeviceId)
    {
        string codeVerifier = _config["VKID:CodeVerifier"]!;
        string redirectUrl = _config["VKID:RedirectUrl"]!;
        string clientId = _config["VKID:ClientId"]!;
        string state = _config["VKID:State"]!;

        FormUrlEncodedContent formContent = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code_verifier", codeVerifier),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("device_id", vkDeviceId),
                new KeyValuePair<string, string>("state", state)
            }
        );

        HttpResponseMessage response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/auth",
            formContent
        );

        if (!response.IsSuccessStatusCode)
        {
            return Errors.VkAuth.Failed;
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(
            responseContent
        )!;

        return tokenResponse.UserId;
    }
}

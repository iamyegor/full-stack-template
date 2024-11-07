using Infrastructure.Auth.Authentication;

namespace Infrastructure.Auth.VkAuth;

public record VkAuthResult(Tokens Tokens, SocialAuthStatus AuthStatus);

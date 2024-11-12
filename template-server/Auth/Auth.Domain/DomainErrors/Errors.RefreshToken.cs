namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class RefreshToken
    {
        public static Error IsInvalid => new Error("refresh_token_is_invalid");
    }
}

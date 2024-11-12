namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class AccessToken
    {
        public static Error IsInvalid => new Error("access.token.invalid");
    }
}

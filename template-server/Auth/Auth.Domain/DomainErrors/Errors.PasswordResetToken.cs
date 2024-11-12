namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class PasswordResetToken
    {
        public static Error IsInvalid => new Error("password.reset.token.is.invalid");
    }
}

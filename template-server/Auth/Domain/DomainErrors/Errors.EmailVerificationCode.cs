namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class EmailVerificationCode
    {
        public static Error HasInvalidLength =>
            new Error("email.verification.code.has.invalid.length");

        public static Error HasInvalidFormat =>
            new Error("email.verification.code.has.invalid.format");

        public static Error IsInvalid => new Error("email.verification.code.is.invalid");
        public static Error IsExpired => new Error("email.verification.code.is.expired");
    }
}

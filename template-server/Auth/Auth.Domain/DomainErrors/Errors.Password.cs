namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class Password
    {
        public static Error HasInvalidSignature => new Error("password.has.invalid.signature");
        public static Error HasInvalidLength => new Error("password.has.invalid.length");
        public static Error IsRequired => new Error("password.is.required");
    }
}

namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class Email
    {
        public static Error IsRequired => new Error("email.is.required");
        public static Error IsTooLong => new Error("email.is.too.long");
        public static Error HasInvalidSignature => new Error("email.has.invalid.signature");
        public static Error IsAlreadyTaken => new Error("email.is.already.taken");
    }
}

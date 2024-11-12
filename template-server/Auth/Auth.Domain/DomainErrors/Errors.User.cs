namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error AlreadyExists => new Error("user.already.exists");
        public static Error NotFound => new Error("user.not.found");
        public static Error InvalidCredentials => new Error("invalid.credentials");
        public static Error EmailAlreadyExists => new Error("user.email.exists");
        public static Error AddEmailError => new Error("user.add.email.error");
    }
}

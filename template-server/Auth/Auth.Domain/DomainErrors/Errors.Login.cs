namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class Login
    {
        public static Error IsAlreadyTaken => new Error("login.is.already.taken");
    }
}

namespace Domain.Common.Preconditions;

public class PreconditionException : Exception
{
    public PreconditionException(string? message)
        : base(message) { }
}

namespace Domain.Common.Preconditions;

public class Precondition
{
    public static void Requires(bool condition, string? message = null)
    {
        if (!condition)
        {
            throw new PreconditionException(message);
        }
    }
}

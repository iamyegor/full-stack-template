using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class PasswordResetToken : ValueObject
{
    protected PasswordResetToken() { }

    private PasswordResetToken(Guid value)
    {
        Value = value;
        ExpiryTime = DateTimeOffset.UtcNow.AddHours(1);
    }

    public Guid Value { get; }
    public DateTimeOffset ExpiryTime { get; }
    public bool IsExpired => DateTimeOffset.UtcNow > ExpiryTime;

    public static PasswordResetToken GenerateRandom()
    {
        return new PasswordResetToken(Guid.NewGuid());
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }

    public static Result<PasswordResetToken, Error> Create(string queryToken)
    {
        if (!Guid.TryParse(queryToken, out _))
        {
            return Errors.PasswordResetToken.IsInvalid;
        }

        return new PasswordResetToken(Guid.Parse(queryToken));
    }
}

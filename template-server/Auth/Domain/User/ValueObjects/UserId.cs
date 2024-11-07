using Domain.Common;

namespace Domain.User.ValueObjects;

public class UserId : ValueObject
{
    public UserId(Guid value)
    {
        Value = value;
    }

    public UserId()
    {
        Value = Guid.NewGuid();
    }

    public Guid Value { get; }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }
}

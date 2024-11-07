using Domain.Common;

namespace Domain.Users.ValueObjects;

public class SubscriptionStatus : ValueObject
{
    public static SubscriptionStatus Free { get; } = new SubscriptionStatus("free", 30);
    public static SubscriptionStatus Plus { get; } = new SubscriptionStatus("plus", 2_000_000);

    public string Value { get; }
    public int MaxMessages { get; }

    private SubscriptionStatus(string value, int maxMessages)
    {
        Value = value;
        MaxMessages = maxMessages;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

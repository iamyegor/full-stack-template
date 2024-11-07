using Domain.Common;

namespace Domain.User.ValueObjects;

public class RefreshToken : ValueObject
{
    protected RefreshToken() { }

    public RefreshToken(string value, DeviceId deviceId)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty or whitespace.", nameof(value));
        }

        Value = value;
        DeviceId = deviceId;
        ExpiryTime = DateTimeOffset.UtcNow.AddDays(7);
    }

    public DeviceId DeviceId { get; }
    public string Value { get; }
    public DateTimeOffset ExpiryTime { get; }
    public bool IsExpired => DateTimeOffset.UtcNow > ExpiryTime;

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }

    public bool Matches(string refreshToken)
    {
        return Value == refreshToken;
    }
}

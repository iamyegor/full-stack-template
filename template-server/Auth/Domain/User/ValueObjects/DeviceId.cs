using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class DeviceId : ValueObject
{
    protected DeviceId() { }

    private DeviceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static Result<DeviceId, Error> Create(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || !Guid.TryParse(input, out Guid deviceId))
        {
            return Errors.DeviceId.IsInvalid;
        }

        return new DeviceId(deviceId);
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }
}

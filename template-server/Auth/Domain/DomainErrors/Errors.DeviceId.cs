namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class DeviceId
    {
        public static Error IsInvalid => new Error("device.id.is.invalid");
    }
}

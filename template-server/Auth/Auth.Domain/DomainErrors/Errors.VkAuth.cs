namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class VkAuth
    {
        public static Error Failed => new Error("vk.auth.failed");
    }
}

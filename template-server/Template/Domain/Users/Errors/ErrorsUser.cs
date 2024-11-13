using Domain.Common;

namespace Domain.Users.Errors;

public static class ErrorsUser
{
    public static Error NotFound => new Error("user.not.found");
    public static Error HasNoModelAccess => new("user.has.no.model.access");
    public static Error ReachedMessageLimit => new("user.reached.message.limit");
}

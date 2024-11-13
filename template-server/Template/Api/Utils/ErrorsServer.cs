using Domain.Common;

namespace Api.Utils;

public static class ErrorsServer
{
    public static Error InternalServerError => new("internal.server.error");
}

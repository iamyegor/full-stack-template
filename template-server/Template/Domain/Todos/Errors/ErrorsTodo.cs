using Domain.Errors;

namespace Domain.Todos.Errors;

public static class ErrorsTodo
{
    public static Error TitleIsRequired => new Error("todo.title.is.required");
    public static Error TitleIsTooLong => new Error("todo.title.is.too.long");
}

using Domain.Common;
using Domain.Todos.Errors;
using NpgsqlTypes;
using XResults;

namespace Domain.Todos;

public class Todo : Entity<Guid>
{
    public string Title { get; private set; }
    public bool Completed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public NpgsqlTsVector SearchVector { get; private set; }

    protected Todo()
        : base(new Guid()) { }

    private Todo(Guid id, string title, bool completed, DateTime createdAt)
        : base(id)
    {
        Title = title;
        Completed = completed;
        CreatedAt = createdAt;
    }

    public static Result<Todo, Error> Create(Guid id, string title, bool completed = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ErrorsTodo.TitleIsRequired;

        if (title.Length > 200)
            return ErrorsTodo.TitleIsTooLong;

        return new Todo(id, title.Trim(), completed, DateTime.UtcNow);
    }

    public void ChangeCompletionStatus(bool completed) => Completed = completed;
}

using Domain.Common;
using Domain.Errors;
using Domain.Todos.Errors;
using XResults;

namespace Domain.Todos;

public class Todo : Entity<Guid>
{
    public string Title { get; private set; }
    public bool Completed { get; private set; }

    protected Todo(Guid? id = null)
        : base(id ?? Guid.NewGuid()) { }

    private Todo(Guid id, string title, bool completed)
        : base(id)
    {
        Title = title;
        Completed = completed;
    }

    public static Result<Todo, Error> Create(Guid id, string title, bool completed = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ErrorsTodo.TitleIsRequired;

        if (title.Length > 200)
            return ErrorsTodo.TitleIsTooLong;

        return new Todo(id, title.Trim(), completed);
    }

    public void ChangeCompletionStatus(bool completed) => Completed = completed;
}

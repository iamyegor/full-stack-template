using Domain.Common;
using Domain.Todos;
using Infrastructure.Data;
using MediatR;
using XResults;

namespace Application.Todos.Commands.AddTodo;

public record AddTodoCommand(string Text) : IRequest<SuccessOr<Error>>;

public class AddTodoCommandHandler : IRequestHandler<AddTodoCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;

    public AddTodoCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SuccessOr<Error>> Handle(
        AddTodoCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<Todo, Error> result = Todo.Create(Guid.NewGuid(), request.Text);

        if (result.IsFailure)
            return result.Error;

        _context.Todos.Add(result.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

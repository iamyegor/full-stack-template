using Domain.Common;
using Domain.Todos;
using Domain.Todos.Errors;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.Todos.Commands.ChangeCompletionStatus;

public record ChangeCompletionStatusCommand(Guid Id, bool Completed) : IRequest<SuccessOr<Error>>;

public class ChangeCompletionStatusCommandHandler
    : IRequestHandler<ChangeCompletionStatusCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;

    public ChangeCompletionStatusCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<SuccessOr<Error>> Handle(
        ChangeCompletionStatusCommand command,
        CancellationToken ct
    )
    {
        Todo? todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (todo == null)
            return ErrorsTodo.NotFound;

        todo.ChangeCompletionStatus(command.Completed);
        await _context.SaveChangesAsync(ct);

        return Result.Ok();
    }
}

using Application.Todos.Queries.GetPagedTodos.Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todos.Queries.GetTodos;

public record GetTodosQuery : IRequest<List<TodoDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<TodoDto>>
{
    private readonly ApplicationContext _context;

    public GetTodosQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<TodoDto>> Handle(GetTodosQuery request, CancellationToken ct)
    {
        List<TodoDto> todos = await _context
            .Todos.OrderBy(t => t.Id)
            .Select(t => new TodoDto(t.Id, t.Title, t.Completed))
            .ToListAsync(ct);

        return todos;
    }
}

using Application.Todos.Queries.Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todos.Queries.GetTodos;

public record GetTodosQuery(string? Search) : IRequest<List<TodoDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<TodoDto>>
{
    private readonly ApplicationContext _context;

    public GetTodosQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<TodoDto>> Handle(GetTodosQuery command, CancellationToken ct)
    {
        List<TodoDto> todos = await _context
            .Todos.OrderByDescending(t => t.CreatedAt)
            .Where(t =>
                command.Search == null
                || t.SearchVector.Matches(
                    EF.Functions.WebSearchToTsQuery("english", command.Search)
                )
            )
            .Select(t => new TodoDto(t.Id, t.Title, t.Completed))
            .AsNoTracking()
            .ToListAsync(ct);

        return todos;
    }
}

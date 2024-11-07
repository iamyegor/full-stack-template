using Application.Todos.Queries.GetPagedTodos.Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Todos.Queries.GetPagedTodos;

public record GetPagedTodosQuery(int Page) : IRequest<PagedTodosDto>;

public class GetPagedTodosQueryHandler : IRequestHandler<GetPagedTodosQuery, PagedTodosDto>
{
    private readonly ApplicationContext _context;

    public GetPagedTodosQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<PagedTodosDto> Handle(GetPagedTodosQuery query, CancellationToken ct)
    {
        const int limit = 5;

        int skip = (query.Page - 1) * limit;

        List<TodoDto> todos = await _context
            .Todos.OrderBy(t => t.Id)
            .Skip(skip)
            .Take(limit + 1)
            .Select(t => new TodoDto(t.Id, t.Title, t.Completed))
            .ToListAsync(ct);

        bool hasNextPage = todos.Count > limit;
        int? nextPage = hasNextPage ? query.Page + 1 : null;

        IEnumerable<TodoDto> pagedTodos = todos.Take(limit);

        return new PagedTodosDto(pagedTodos, nextPage);
    }
}

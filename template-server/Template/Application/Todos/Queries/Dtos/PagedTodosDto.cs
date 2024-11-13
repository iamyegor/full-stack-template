namespace Application.Todos.Queries.Dtos;

public record PagedTodosDto(IEnumerable<TodoDto> Todos, int? NextPage);

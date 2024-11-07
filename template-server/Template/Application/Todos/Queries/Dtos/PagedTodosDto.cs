namespace Application.Todos.Queries.GetPagedTodos.Dtos;

public record PagedTodosDto(IEnumerable<TodoDto> Todos, int? NextPage);

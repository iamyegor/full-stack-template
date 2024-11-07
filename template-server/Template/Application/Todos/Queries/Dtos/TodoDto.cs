namespace Application.Todos.Queries.GetPagedTodos.Dtos;

public record TodoDto(Guid Id, string Title, bool Completed);
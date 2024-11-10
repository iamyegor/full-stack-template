using Api.Controllers.Common;
using Api.Dtos;
using Application.Todos.Commands.ChangeCompletionStatus;
using Application.Todos.Queries.GetPagedTodos;
using Application.Todos.Queries.GetPagedTodos.Dtos;
using Application.Todos.Queries.GetTodos;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("todos")]
public class TodosController : ApplicationController
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("paged/{page}")]
    public async Task<IActionResult> GetPagedTodos(int page = 1)
    {
        PagedTodosDto todos = await _mediator.Send(new GetPagedTodosQuery(page));

        return Ok(todos);
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos(string? search)
    {
        List<TodoDto> todos = await _mediator.Send(new GetTodosQuery(search));

        return Ok(todos);
    }

    [HttpPost("{id}/change-completion-status")]
    public async Task<IActionResult> ChangeCompletionStatus(Guid id, ChangeCompletionStatusdDto dto)
    {
        SuccessOr<Error> result = await _mediator.Send(
            new ChangeCompletionStatusCommand(id, dto.Completed)
        );

        return FromResult(result);
    }
}

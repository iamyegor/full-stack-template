using Api.Controllers.Common;
using Application.Todos.Queries.GetPagedTodos;
using Application.Todos.Queries.GetPagedTodos.Dtos;
using Application.Todos.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<PagedTodosDto>> GetPagedTodos(int page = 1)
    {
        throw new Exception("Fake exception");
        PagedTodosDto todos = await _mediator.Send(new GetPagedTodosQuery(page));

        return Ok(todos);
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>> GetTodos()
    {
        List<TodoDto> todos = await _mediator.Send(new GetTodosQuery());

        return Ok(todos);
    }
}

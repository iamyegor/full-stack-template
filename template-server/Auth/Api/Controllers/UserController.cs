using Api.Controllers.Common;
using Api.Dtos;
using Application.User.Commands.AddEmailCommand;
using Application.User.Queries.IsAuthenticated;
using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Cookies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("user")]
public class UserController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly UserIdExtractor _userIdExtractor;

    public UserController(UserIdExtractor userIdExtractor, IMediator mediator)
    {
        _userIdExtractor = userIdExtractor;
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("name-and-email")]
    public async Task<IActionResult> AddNameAndEmail(AddEmailDto dto)
    {
        Result<UserId, Error> userIdOrError = _userIdExtractor.ExtractUserId(Request.Cookies);
        if (userIdOrError.IsFailure)
        {
            return Problem(userIdOrError.Error);
        }

        AddNameAndEmailCommand command = new AddNameAndEmailCommand(
            userIdOrError,
            dto.Name,
            dto.Email
        );
        SuccessOr<Error> result = await _mediator.Send(command);

        return FromResult(result);
    }

    [Authorize]
    [HttpGet("is-authenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        Result<UserId, Error> userId = _userIdExtractor.ExtractUserId(Request.Cookies);
        if (userId.IsFailure)
        {
            return Problem(userId.Error);
        }

        IsAuthenticatedQuery query = new IsAuthenticatedQuery(userId);
        Result<bool, Error> isAuthenticatedOrError = await _mediator.Send(query);
        if (isAuthenticatedOrError.IsFailure)
        {
            return Problem(isAuthenticatedOrError.Error);
        }

        if (isAuthenticatedOrError.Value)
        {
            return NoContent();
        }
        else
        {
            return Unauthorized();
        }
    }
}

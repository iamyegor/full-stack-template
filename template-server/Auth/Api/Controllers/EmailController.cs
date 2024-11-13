using Api.Controllers.Common;
using Api.Dtos;
using Application.Email.Commands.ConfirmEmail;
using Application.Email.Commands.ResendEmailCode;
using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("email")]
public class EmailController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly UserIdExtractor _userIdExtractor;

    public EmailController(UserIdExtractor userIdExtractor, IMediator mediator)
    {
        _userIdExtractor = userIdExtractor;
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmEmail(VerifyEmailDto dto)
    { 
        Result<UserId, Error> userIdOrError = _userIdExtractor.ExtractUserId(Request.Cookies);
        if (userIdOrError.IsFailure)
        {
            return Problem(userIdOrError.Error);
        }

        ConfirmEmailCommand confirmEmailCommand = new ConfirmEmailCommand(
            userIdOrError.Value,
            dto.Code
        );
        SuccessOr<Error> result = await _mediator.Send(confirmEmailCommand);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("resend-email-code")]
    public async Task<IActionResult> ResendEmailCode()
    {
        Result<UserId, Error> userIdOrError = _userIdExtractor.ExtractUserId(Request.Cookies);
        if (userIdOrError.IsFailure)
        {
            return Problem(userIdOrError.Error);
        }

        ResendEmailCodeCommand resendEmailCodeCommand = new ResendEmailCodeCommand(
            userIdOrError.Value
        );
        SuccessOr<Error> result = await _mediator.Send(resendEmailCodeCommand);

        return FromResult(result);
    }
}

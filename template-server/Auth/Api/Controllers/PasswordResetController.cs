using Api.Controllers.Common;
using Api.Dtos;
using Application.User.Commands.RequestPasswordReset;
using Application.User.Commands.ResetPassword;
using Domain.DomainErrors;
using Infrastructure.Auth.Authentication;
using Infrastructure.Cookies.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Auth;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("password")]
public class PasswordResetController : ApplicationController
{
    private readonly IMediator _mediator;

    public PasswordResetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetDto dto)
    {
        RequestPasswordResetCommand command = new RequestPasswordResetCommand(dto.Email);
        SuccessOr<Error> result = await _mediator.Send(command);
        return FromResult(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        Request.Cookies.TryGetValue(CookiesSettings.DeviceId.Name, out string? deviceId);
        ResetPasswordCommand command = new ResetPasswordCommand(
            dto.Token,
            dto.NewPassword,
            deviceId
        );
        Result<Tokens, Error> tokensOrError = await _mediator.Send(command);
        if (tokensOrError.IsFailure)
        {
            return Problem(tokensOrError.Error);
        }

        HttpContext.Response.Cookies.Append(tokensOrError.Value);

        return Ok();
    }
}

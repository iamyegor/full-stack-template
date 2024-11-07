using Api.Controllers.Common;
using Api.Dtos;
using Application.User.Commands.SignIn;
using Application.User.Commands.SignUp;
using Domain.DomainErrors;
using Infrastructure.Auth.Authentication;
using Infrastructure.Cookies.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Auth;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ApplicationController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignupDto dto)
    {
        Request.Cookies.TryGetValue(CookiesSettings.DeviceId.Name, out string? deviceId);
        SignUpCommand signUpCommand = new SignUpCommand(dto.Email, dto.Password, deviceId);

        Result<Tokens, Error> tokensOrError = await _mediator.Send(signUpCommand);
        if (tokensOrError.IsFailure)
        {
            return Problem(tokensOrError.Error);
        }

        Response.Cookies.Append(tokensOrError.Value);

        return Ok();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> Signin(SigninDto dto)
    {
        Request.Cookies.TryGetValue(CookiesSettings.DeviceId.Name, out string? deviceId);
        SignInCommand signInCommand = new SignInCommand(dto.Email, dto.Password, deviceId);

        Result<Tokens, Error> tokensOrError = await _mediator.Send(signInCommand);
        if (tokensOrError.IsFailure)
        {
            return Problem(tokensOrError.Error);
        }

        HttpContext.Response.Cookies.Append(tokensOrError.Value);

        return Ok();
    }

    [HttpPost("log-out"), Authorize]
    public IActionResult LogOut()
    {
        Response.Cookies.Delete(CookiesSettings.AccessToken.Name, CookiesSettings.AccessToken.Options);
        Response.Cookies.Delete(CookiesSettings.RefreshToken.Name, CookiesSettings.RefreshToken.Options);

        return Ok();
    }
}

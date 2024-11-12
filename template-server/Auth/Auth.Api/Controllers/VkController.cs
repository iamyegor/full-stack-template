using Api.Controllers.Common;
using Api.Dtos;
using Application.SignIn.Commands.SignInWithVk;
using Domain.DomainErrors;
using Infrastructure.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using SharedKernel.Auth;
using XResults;

namespace Api.Controllers;

[ApiController]
[Route("vk")]
public class VkController : ApplicationController
{
    private readonly IMediator _mediator;

    public VkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInWithVk(VkSignInDto dto)
    {
        Request.Cookies.TryGetValue(CookiesSettings.DeviceId.Name, out string? deviceId);
        SignInWithVkCommand command = new SignInWithVkCommand(dto.Code, dto.VkDeviceId, deviceId);

        Result<SignInWithVkResult, Error> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        Response.Cookies.Append(result.Value.Tokens);

        return Ok(new { AuthStatus = result.Value.AuthStatus.GetDisplayName() });
    }
}

using System.Net;
using Domain.DomainErrors;
using Microsoft.AspNetCore.Mvc;
using XResults;

namespace Api.Controllers.Common;

public class ApplicationController : ControllerBase
{
    protected IActionResult Problem(Error error)
    {
        return new ErrorResult(error, HttpStatusCode.BadRequest);
    }

    protected IActionResult FromResult(SuccessOr<Error> result)
    {
        return result.IsSuccess ? NoContent() : Problem(result.Error);
    }

    protected IActionResult FromResult<TValue>(Result<TValue, Error> result)
    {
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }
}

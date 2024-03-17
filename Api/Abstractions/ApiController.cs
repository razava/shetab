using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Abstractions;

[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected ActionResult Problem(Result result)
    {
        return Problem(
            String.Join("\r\n", result.Errors.Select(e => e.Message).ToList()),
            statusCode: StatusCodes.Status500InternalServerError);
    }

    protected ActionResult Ok<T>(Result<T> result)
    {
        var message = "This is a test for Hossein";
        return Ok(new ResponseWrapper<T>(message, result.Value));
    }

    protected ActionResult CreatedAtAction<T>(string? actionName, object? routeValues, T? value)
    {
        var message = "This is a test for Hossein";
        return CreatedAtAction(actionName, routeValues, new ResponseWrapper<T>(message, value));
    }
    public record ResponseWrapper<T>(string Message, T? Data);
}

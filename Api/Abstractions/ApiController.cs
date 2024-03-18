using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        return base.Problem(
            String.Join("\r\n", result.Errors.Select(e => e.Message).ToList()),
            statusCode: StatusCodes.Status500InternalServerError);
    }

    protected ActionResult Ok<T>(Result<T> result)
    {
        var message = "This is a test for Hossein";

        if(result.Value is IPagedList pagination)
        {
            Response.AddPaginationHeaders(pagination.Meta);
        }
        return base.Ok(new ResponseWrapper<T>(message, result.Value));
    }

    protected ActionResult CreatedAtAction<T>(string? actionName, object? routeValues, Result<T> result)
    {
        var message = "This is a test for Hossein";
        return base.CreatedAtAction(actionName, routeValues, new ResponseWrapper<T>(message, result.Value));
    }

    protected ActionResult StatusCode<T>(int statusCode, Result<T> result)
    {
        var message = "This is a test for Hossein";
        return base.StatusCode(statusCode, new ResponseWrapper<T>(message, result.Value));
    }
    public record ResponseWrapper<T>(string Message, T? Data);
}

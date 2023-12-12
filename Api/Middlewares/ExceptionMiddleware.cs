using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private RequestDelegate Next { get; }

    public ExceptionMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails()
            {
                Status = context.Response.StatusCode,
                Detail = "One or more validation errors has occurred",
                Instance = "",
                Title = "Validation error",
                Type = "ValidationFailure"
            };

            if (ex.Errors is not null)
            {
                var errors = ex.Errors
                    .Select(e => new {
                        PropertyName = e.PropertyName,
                        ErrorMessage = e.ErrorMessage,
                        AttemptedValue = e.AttemptedValue,
                        ErrorCode = e.ErrorCode
                    });
                problemDetails.Extensions["errors"] = errors;  //= ex.Errors;
            }

            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);

        }
        catch (Exception ex)
        {

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails()
            {
                Status = context.Response.StatusCode,
                Detail = ex.Message,
                Instance = "",
                Title = "Internal server Error - Something went wrong",
                Type = "Error"
            };

            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);

        }
    }

}



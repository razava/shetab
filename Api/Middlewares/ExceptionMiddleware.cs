using Application.Common.Exceptions;
using Domain.Exceptions;
using FluentValidation;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
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
            var problemDetails = new ProblemDetails();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            problemDetails.Detail = ex.Message;
            problemDetails.Instance = "";
            problemDetails.Type = "Error";
            problemDetails.Title = ex.GetType().Name;

            switch (ex)
            {
                case ServerNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = ex.InnerException!.GetType().Name;
                    break;
                case InvalidCaptchaException:
                case NullAssignedRoleException:
                case InvalidUsernameException:
                case UserAlreadyExsistsException:
                case InvalidLoginException:
                case AccessDeniedException:
                case CommentHasReplyException:
                case LoopMadeException:
                case ExecutiveOnlyLimitException:
                case InvalidVerificationCodeException:
                case ForbidNullProcessException:
                case CategoryHaveNoAssignedProcess:
                case ParticipationMoreThanOnceException:
                case NullAnswerTextException:
                case OneChoiceAnswerLimitException:
                case InvalidChoiceException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                case NullUserRolesException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case CreationFailedException:
                case SaveImageFailedException:
                case AttachmentsFailureException:
                case SendSmsException:
                case ResetPasswordTokenGenerationFailedException:
                case InvalidCityException:
                case DuplicateInstanceException:
                case ForbidNullParentCategoryException:
                case ForbidNullRolesException:
                case NullConnectionStringException:
                case NullJwtSecretException:
                case NullJwtValidIssuerException:
                case NullJwtValidAudienceException:
                case MessageBrokerConfigurationsNotFoundException:
                case ForbidNullRoleException:
                case NullActorException:
                case ForbidNullStageException:
                case RoleAssignmentFailedException:
                case NotLoadedProcessException:
                case NullCurrentStageException:
                case NullStageException:
                case BotNotFoundException:
                case ForbidNullTransitionException:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal server Error - Something went wrong";
                    break;
            }

            problemDetails.Status = context.Response.StatusCode;
            
            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(problemDetailsJson);

        }
    }

}



using Application.Common.Exceptions;
using Domain.Exceptions;
using FluentValidation;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private RequestDelegate Next { get; }
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        Next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
            /*
            _logger.LogError(ex, "Exception occurred : Validation error : {Message} {@Errors} ",
                ex.Message,
                ex.Errors);

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
            */
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred : {Message}", ex.Message);

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
                case UserCreationFailedException:
                case RoleAssignmentFailedException:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails = HandleIdentityErrors(ex, problemDetails);
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
                case BadRequestException:
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


    private async Task<ActionResult<bool>> HandleValidationException(HttpContext context, ValidationException ex)
    {
        _logger.LogError(ex, "Exception occurred : Validation error : {Message} {@Errors} ",
                ex.Message,
                ex.Errors);

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
        return true;
    }

    
    private ProblemDetails HandleIdentityErrors(Exception ex, ProblemDetails problemDetails)
    {
        if(ex.Data["Errors"] != null)
        {
            var errors = (List<IdentityError>)ex.Data["Errors"]!;
            string errorMessages = "";
            bool isOneOf = true;
            var errorDetails = JsonSerializer.Serialize(errors);
            _logger.LogError(ex, "[Operation Failed, errors: {Errors}", errorDetails);

            foreach (var error in errors)
            {
                switch (error.Code)
                {
                    case nameof(IdentityErrorDescriber.DuplicateUserName):
                        error.Description = "نام کاربری تکراری است.";
                        break;
                    case nameof(IdentityErrorDescriber.InvalidUserName):
                        error.Description = "نام کاربری نامعتبر است.";
                        break;
                    case nameof(IdentityErrorDescriber.PasswordTooShort):  //if other limits on password added to authentication configuration,other checks must add :|
                        error.Description = "رمز عبور حداقل باید 6 کارکتر باشد.";
                        break;
                    case nameof(IdentityErrorDescriber.PasswordMismatch):
                        error.Description = "پسورد همخوانی ندارد.";
                        break;
                    case nameof(IdentityErrorDescriber.InvalidToken):
                        error.Description = "توکن نامعتبر است.";
                        break;
                    case nameof(IdentityErrorDescriber.InvalidRoleName):
                        error.Description = "نقش نامعتبر.";
                        break;
                    case nameof(IdentityErrorDescriber.DuplicateRoleName):
                        error.Description = "نقش تکراری.";
                        break;
                    default:
                        isOneOf = false;
                        break;
                }
                if (isOneOf)
                    errorMessages = errorMessages + error.Description;
            }

            problemDetails.Extensions["errors"] = errors;
            problemDetails.Detail = errorMessages;
            
        }
        return problemDetails;
    }
}



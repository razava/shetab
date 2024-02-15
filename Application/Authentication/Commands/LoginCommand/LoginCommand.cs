using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LoginCommand;

public sealed record LoginCommand(
    string Username,
    string Password,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<LoginResponse>>;
public record LoginResponse(AuthToken? AuthToken, string? VerificationToken);

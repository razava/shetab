using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LoginCommand;

public sealed record TwoFactorLoginCommand(
    string OtpToken,
    string Code) :IRequest<Result<AuthToken>>;

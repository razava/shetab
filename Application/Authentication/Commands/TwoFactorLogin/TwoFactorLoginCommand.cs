using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.TwoFactorLogin;

public sealed record TwoFactorLoginCommand(
    string OtpToken,
    string Code) : IRequest<Result<AuthToken>>;

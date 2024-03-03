using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LoginMyYazdCommand;

public sealed record LoginMyYazdCommand(
    string Code) : IRequest<Result<AuthToken>>;


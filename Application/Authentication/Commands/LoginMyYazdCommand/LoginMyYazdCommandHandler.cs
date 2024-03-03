using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LoginMyYazdCommand;

internal sealed class LoginMyYazdCommandHandler(
    IAuthenticationService authenticationService)
    : IRequestHandler<LoginMyYazdCommand, Result<AuthToken>>
{

    public async Task<Result<AuthToken>> Handle(
        LoginMyYazdCommand request,
        CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginMyYazd(request.Code);
        return result;
    }
}


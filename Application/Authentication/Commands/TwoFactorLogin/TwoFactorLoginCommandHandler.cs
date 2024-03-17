using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.TwoFactorLogin;

internal sealed class TwoFactorLoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<TwoFactorLoginCommand, Result<AuthToken>>
{
    public async Task<Result<AuthToken>> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.VerifyOtp(request.OtpToken, request.Code);
        return result;
    }
}


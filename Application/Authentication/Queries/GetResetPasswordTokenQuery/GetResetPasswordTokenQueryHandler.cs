using Application.Common.Exceptions;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.GetResetPasswordTokenQuery;

internal sealed class GetResetPasswordTokenQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider) : IRequestHandler<GetResetPasswordTokenQuery, Result<string>>
{
    
    public async Task<Result<string>> Handle(GetResetPasswordTokenQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }
        var resetPasswordToken = await authenticationService.GetResetPasswordToken(request.Username, request.verificationCode);

        return resetPasswordToken;
    }
}

using Application.Common.Exceptions;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.ResetPasswordCommand;

internal sealed class ResetPasswordCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider) : IRequestHandler<ResetPasswordCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }
        var isSucceeded = await authenticationService.ResetPassword(request.Username, request.ResetPasswordToken, request.NewPassword);
        if (!isSucceeded)
            return AuthenticateErrors.ChangePasswordFailed;

        return isSucceeded;
    }
}

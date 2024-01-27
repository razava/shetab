using Application.Common.Exceptions;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.VerifyPhoneNumberCommand;

internal sealed class VerifyPhoneNumberCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider) : IRequestHandler<VerifyPhoneNumberCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(VerifyPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }
        var isVerified = await authenticationService.VerifyPhoneNumber(request.Username, request.verificationCode);
        if (!isVerified)
            return AuthenticateErrors.VerificationFailed;

        return isVerified;
    }
}

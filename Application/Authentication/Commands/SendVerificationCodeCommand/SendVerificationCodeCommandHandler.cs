using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.SendVerificationCodeCommand;

internal sealed class SendVerificationCodeCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService) : IRequestHandler<SendVerificationCodeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }
        var verificationCode = await authenticationService.GetVerificationCode(request.Username);
        try
        {
            await communicationService.SendVerificationAsync(request.Username, verificationCode);
        }
        catch
        {
            return AuthenticateErrors.SendSms;
        }

        return true;
    }
}

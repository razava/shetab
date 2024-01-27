using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.ForgotPasswordQuery;

internal sealed class ForgotPasswordQueryHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService) : IRequestHandler<ForgotPasswordQuery, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
                return AuthenticateErrors.InvalidCaptcha;
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

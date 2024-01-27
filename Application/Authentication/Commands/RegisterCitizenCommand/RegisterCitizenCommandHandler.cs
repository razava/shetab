using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RegisterCitizenCommand;

internal class RegisterCitizenCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService) : IRequestHandler<RegisterCitizenCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(RegisterCitizenCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }

        bool result;
        try
        {
            result = await authenticationService.RegisterCitizen(request.Username, request.Password);
            if (result)
            {
                var verificationCode = await authenticationService.GetVerificationCode(request.Username);
                try
                {
                    await communicationService.SendVerificationAsync(request.Username, verificationCode);
                }
                catch
                {
                    return AuthenticateErrors.SendSms;
                }
                //result = true;
                return true;
            }
        }
        catch
        {
            return AuthenticateErrors.UserCreationFailed;
        }

        return AuthenticateErrors.UserCreationFailed;
        //return result;
    }
}

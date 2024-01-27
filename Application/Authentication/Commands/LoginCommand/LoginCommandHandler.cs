using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class LoginCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ICommunicationService communicationService) : IRequestHandler<LoginCommand, Result<LoginResultModel>>
{
    
    public async Task<Result<LoginResultModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if(request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticateErrors.InvalidCaptcha;
            }
        }

        LoginResultModel? result;
        try
        {
            result = await authenticationService.Login(request.Username, request.Password, request.VerificationCode);
        }
        catch (PhoneNumberNotConfirmedException)
        {
            var verificationCode = await authenticationService.GetVerificationCode(request.Username);
            try
            {
                await communicationService.SendVerificationAsync(request.Username, verificationCode);
            }
            catch
            {
                return CommunicationErrors.SendSms;
            }
            result = new LoginResultModel("", true);
        }
        return result;
    }
}

